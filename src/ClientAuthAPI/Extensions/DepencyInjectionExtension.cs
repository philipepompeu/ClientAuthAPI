
using ClientAuthAPI.Documents;
using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Models;
using ClientAuthAPI.Repositories;
using ClientAuthAPI.Services;
using MongoDB.Driver;

namespace ClientAuthAPI.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<MongoService>();
        services.AddScoped<IMongoCollection<ClientDocument>>(sp =>
        {
            var mongo = sp.GetRequiredService<MongoService>();
            return mongo.GetCollection<ClientDocument>("clients");
        });

        services.AddScoped<IMongoCollection<UserDocument>>(sp =>
        {
            var mongo = sp.GetRequiredService<MongoService>();
            return mongo.GetCollection<UserDocument>("users");
        });
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}