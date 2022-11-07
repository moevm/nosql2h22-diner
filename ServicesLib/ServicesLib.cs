using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServicesLib.ModelServices;
using UtilsLib.Configurations;

namespace ServicesLib;

public static class ServicesLib
{
    public static IServiceCollection AddServicesLib(this IServiceCollection services, ConfigurationManager configuration)
    {
        // ModelServices
        services.AddScoped<UserService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<DishService>();
        services.AddScoped<CommentService>();
        services.AddScoped<ResourceService>();
        services.AddScoped<DishResourceService>();
        services.AddScoped<ShiftService>();
        services.AddScoped<WeekService>();
     
        return services;
    }
}