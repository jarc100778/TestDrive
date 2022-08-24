using CarCatalogService.AsyncDataServices;
using CarCatalogService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarCatalogService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("CarCatalogConn")));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<ICarRepo, CarRepo>();
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

            // accepts any access token issued by identity server
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:7161";
                    options.Audience = "CarCatalogService";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            // adds an authorization policy to make sure the token is for scope 'cars.read'
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("cars.read", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(p =>
                            (p.Type == "scope" && p.Value == "CarCatalogService.read") ||
                            (p.Type == "scope" && p.Value == "CarCatalogService.all")
                        ));
                });
                options.AddPolicy("cars.all", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "CarCatalogService.all");
                });
            });


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsProduction())
            {
                Console.WriteLine("--> Using Production Environment");
            }
            else
            {
                Console.WriteLine("--> Using Dev Db");
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            PrepDb.PrepPopulation(app, app.Environment.IsProduction());

            app.Run();
        }
    }
}