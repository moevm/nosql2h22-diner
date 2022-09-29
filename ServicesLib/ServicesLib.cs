using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesLib.ModelServices;
using UtilsLib.Configurations;

namespace ServicesLib;

public static class ServicesLib
{
    public static IServiceCollection AddServicesLib(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<UserDbConfig>(configuration.GetSection("UserDbConfig"));

        services.AddScoped<UserService>();
        
        return services;
    }
}