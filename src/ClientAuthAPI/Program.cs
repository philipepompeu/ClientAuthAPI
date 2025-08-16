using ClientAuthAPI;
using ClientAuthAPI.Extensions;
using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MongoService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var mongoSection = builder.Configuration.GetSection("MongoDB");

builder.Services.Configure<JwtSettings>(jwtSection);
builder.Services.Configure<MongoSettings>(mongoSection);

var app = builder.Build();

app.MapClientEndpoints();
app.MapAuthEndpoints();

app.Run();
