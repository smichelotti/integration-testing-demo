using Microsoft.Azure.Cosmos;

namespace ContactsApi.Infrastructure;

public static class ConfigExtensions
{
    public static void ConfigureApiServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConfiguration>(config);
        services.AddTransient<ApiServiceFactory>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddAutoMapper(typeof(Program));
        services.AddHttpClient();
        services.AddSingleton(p => p.GetService<ApiServiceFactory>().CreateAndInitializeCosmosClientAsync());
        services.AddScoped<CosmosContext>();
        services.AddTransient<IGeoLocationClient, GeoLocationClient>();
    }

    private class ApiServiceFactory
    {
        private readonly IConfiguration config;

        public ApiServiceFactory(IConfiguration config) => this.config = config;
        
        public CosmosClient CreateAndInitializeCosmosClientAsync()
        {
            List<(string, string)> containers = new()
            {
                (config["CosmosConfig:DbName"], config["CosmosConfig:entityContainerName"])
            };

            var clientOptions = new CosmosClientOptions()
            {
                // Important: IF Property Naming strategy is changed, Ensure "LiteralDictionary" class is also updated to inherit from relevant naming strategy
                SerializerOptions = new CosmosSerializationOptions
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                },
                AllowBulkExecution = true,
            };

            return CosmosClient.CreateAndInitializeAsync(this.config.GetConnectionString("cosmos-conn"), containers, clientOptions).GetAwaiter().GetResult();
        }
    }
}