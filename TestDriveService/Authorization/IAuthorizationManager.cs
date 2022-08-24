using IdentityModel.Client;

namespace TestDriveService.Authorization
{
    public interface IAuthorizationManager
    {
        //TokenResponse TokenInfo { get; set; }
//        Task<string?> GetTokenAsync();
        Task<TokenResponse?> GetTokenAsync();
        Task<TokenResponse?> RenewTokenAsync();
    }
}
