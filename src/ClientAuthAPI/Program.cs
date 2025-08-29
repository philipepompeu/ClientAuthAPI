using System.Text;
using ClientAuthAPI.Application;
using ClientAuthAPI.Extensions;
using ClientAuthAPI.Infrastructure.Mongo;
using ClientAuthAPI.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddSecurityInfraServices();        

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtSettings = jwtSection.Get<JwtSettings>();
var mongoSection = builder.Configuration.GetSection("MongoDB");
builder.Services.Configure<JwtSettings>(jwtSection);
builder.Services.Configure<MongoSettings>(mongoSection);

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
        policy.RequireClaim("client_id"));
});

var app = builder.Build();

app.MapClientEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
