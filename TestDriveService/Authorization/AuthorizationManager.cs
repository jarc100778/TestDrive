using IdentityModel;
using IdentityModel.Client;

namespace TestDriveService.Authorization
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private TokenResponse? _tokenInfo;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public AuthorizationManager(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// получить Токен от IdentityServer 
        /// </summary>
        public async Task<TokenResponse?> GetTokenAsync()
        {
            if(_tokenInfo !=  null) return _tokenInfo;

            using var scope = _scopeFactory.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient();


            // запрос инфы об AuthorizationService
            var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync(_configuration["AuthorizationService"]);
            if (discoveryDocument.IsError)
            {
                Console.WriteLine(discoveryDocument.Error);
                return null;
            }
            // запрос токена
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                // запрос прав на чтение данных от сервиса CarCatalogService
                Scope = "CarCatalogService.read"
//                Scope = "CarCatalogService.all"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }
            _tokenInfo = tokenResponse;

            return _tokenInfo;
        }

        public async Task<TokenResponse?> RenewTokenAsync()
        {
            _tokenInfo = null;

            return await GetTokenAsync();
        }
    }
}
