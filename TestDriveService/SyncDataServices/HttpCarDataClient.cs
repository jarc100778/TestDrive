using IdentityModel.Client;
using System.Net;
using System.Text;
using System.Text.Json;
using TestDriveService.Authorization;
using TestDriveService.Dtos;

namespace TestDriveService.SyncDataServices
{
    public class HttpCarDataClient : ICarDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationManager _authorizationManager;

        public HttpCarDataClient(HttpClient httpClient, IConfiguration configuration, IAuthorizationManager authorizationManager)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _authorizationManager = authorizationManager;
        }

        public async Task<CarImportDto?> CreateCar(CarCreateDto carCreateDto)
        {
            var tokenResponse = await _authorizationManager.GetTokenAsync();
            _httpClient.SetBearerToken(tokenResponse.AccessToken);

            var url = _configuration["CarCatalogService"] + "/api/cars";
            var httpContent = new StringContent(
                JsonSerializer.Serialize(carCreateDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(requestUri: url, content: httpContent);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("--> Token is expired ! Requesting new token");
                tokenResponse = await _authorizationManager.RenewTokenAsync();
                _httpClient.SetBearerToken(tokenResponse.AccessToken);
                response = await _httpClient.PostAsync(requestUri: url, content: httpContent);
            }

            CarImportDto? car = null;
            if (response.StatusCode == HttpStatusCode.Created )
            {
                Console.WriteLine("--> new car Created by CarCatalogService ! ");
                car = await response.Content.ReadFromJsonAsync<CarImportDto>();
            }
            else
            {
                Console.WriteLine($"--> new car NOT created by CarCatalogService ! StatusCode: {response.StatusCode} ");
            }

            return car;
        }

        public async Task<IEnumerable<CarImportDto>?> GetAllCars()
        {
            var tokenResponse = await _authorizationManager.GetTokenAsync();
            (HttpStatusCode httpStatusCode, IEnumerable<CarImportDto>? carImportDtos) = await RequestToCarCatalogService(tokenResponse);

            switch(httpStatusCode)
            {
                case HttpStatusCode.OK: 
                    return carImportDtos;

                case HttpStatusCode.Unauthorized:
                    Console.WriteLine("--> Token is expired ! Requesting new token");
                    tokenResponse = await _authorizationManager.RenewTokenAsync();
                    (httpStatusCode, carImportDtos) = await RequestToCarCatalogService(tokenResponse);
                    return carImportDtos;

                default:
                    return null;
            }

        }

        // Http-запрос на CarCatalogService
        private async Task<(HttpStatusCode httpStatusCode, IEnumerable<CarImportDto>? carImportDtos)> RequestToCarCatalogService(TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
            {
                Console.WriteLine("--> Token is null !");
                return (HttpStatusCode.BadRequest, null);
            }

            _httpClient.SetBearerToken(tokenResponse.AccessToken);
            var url = _configuration["CarCatalogService"] + "/api/cars";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync GET to CarCatalogService was OK!");
                var carsDto = await response.Content.ReadFromJsonAsync<IEnumerable<CarImportDto>>();
                return (response.StatusCode, carsDto);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return (HttpStatusCode.Unauthorized, null);
            }
            else
            {
                Console.WriteLine("--> Sync GET to CarCatalogService was NOT OK!");
                return (response.StatusCode, null);
            }
        }
    }

}
