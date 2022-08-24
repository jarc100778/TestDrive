using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestDriveService.AsyncDataServices;
using TestDriveService.Authorization;
using TestDriveService.Data;
using TestDriveService.EventProcessing;
using TestDriveService.SyncDataServices;

namespace TestDriveService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("TestDriveConn")));
            builder.Services.AddScoped<ICarRepo, CarRepo>();
            builder.Services.AddScoped<ITestDriveOrderRepo, TestDriveOrderRepo>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHostedService<MessageBusSubscriber>();
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
            builder.Services.AddHttpClient<ICarDataClient, HttpCarDataClient>();

            builder.Services.AddSingleton<IAuthorizationManager, AuthorizationManager>();
            builder.Services.AddHttpClient();

            // accepts any access token issued by identity server
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:7161";
                    options.Audience = "TestDriveService";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };
                    options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(1);  
                });
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}