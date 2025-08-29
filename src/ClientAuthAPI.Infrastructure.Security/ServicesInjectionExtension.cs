using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Infrastructure.Security.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClientAuthAPI.Infrastructure.Security;

public static class ServicesInjectionExtension
{
    public static IServiceCollection AddSecurityInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthTokenService, AuthTokenService>();
        return services;
    }
}