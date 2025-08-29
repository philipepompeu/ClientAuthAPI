using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClientAuthAPI.Application;

public static class ServicesInjectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        //services.AddScoped<IAuthTokenService, AuthTokenService>();

        return services;
    }
}