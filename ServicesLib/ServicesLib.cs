using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesLib.ModelServices;
using UtilsLib.Configurations;

namespace ServicesLib;

public static class ServicesLib
{
    public static IServiceCollection AddServicesLib(this IServiceCollection services, ConfigurationManager configuration)
    {
        // ModelConfigs
        services.Configure<UserDbConfig>(configuration.GetSection("UserDbConfig"));
        services.Configure<UserDbConfig>(configuration.GetSection("PaymentDbConfig"));
        services.Configure<UserDbConfig>(configuration.GetSection("DishDbConfig"));
        services.Configure<UserDbConfig>(configuration.GetSection("ShiftDbConfig"));
        services.Configure<UserDbConfig>(configuration.GetSection("CommentDbConfig"));
        services.Configure<UserDbConfig>(configuration.GetSection("DishResourceDbConfig"));
        services.Configure<UserDbConfig>(configuration.GetSection("ResourceDbConfig"));
        
        // ModelServices
        services.AddScoped<UserService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<DishService>();
        services.AddScoped<CommentService>();
        services.AddScoped<ResourceService>();
        services.AddScoped<DishResourceService>();
        services.AddScoped<ShiftService>();
     
        return services;
    }
}