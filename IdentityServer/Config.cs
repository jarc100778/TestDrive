using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                //new ApiScope("TestDriveService.all", "TestDriveService all rights"),
                //new ApiScope("TestDriveService.read", "TestDriveService read only"),
                new ApiScope("CarCatalogService.all", "CarCatalogService all rights"),
                new ApiScope("CarCatalogService.read", "CarCatalogService read only"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "TestDriveService",
                    ClientSecrets =
                    {
                        new Secret("secret_TestDriveService".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "CarCatalogService.read" },
//                    AllowedScopes = { "CarCatalogService.all" },
                    AccessTokenLifetime = 15, 
                }
            };
    }
}
