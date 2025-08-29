

using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Application.Services;
using ClientAuthAPI.Domain.Interfaces;
using ClientAuthAPI.Infrastructure.Mongo.Documents;
using ClientAuthAPI.Infrastructure.Mongo.Repositories;
using ClientAuthAPI.Infrastructure.Security.Services;
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
    
}