using System.Security.Claims;
using System.Text;
using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ClientAuthAPI.Extensions;

public static class RoutingExtension
{
    public static WebApplication MapClientEndpoints(this WebApplication app)
    {
        app.MapPost("/clients", async (ClientViewModel request, IClientService clientService) =>
        {
            var client = await clientService.ProvisionClientAsync(request);

            return Results.Created($"/clients/{client.ClientId}", new
            {
                client.ClientId,
                client.ClientSecret
            });
        });

        return app;
    }
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/token", async (HttpRequest request, IAuthService authService) =>
        {
            if (!request.Headers.ContainsKey("Authorization"))
                return Results.Unauthorized();

            var authHeader = request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return Results.Unauthorized();

            var base64Credentials = authHeader.Substring("Basic ".Length).Trim();
            string decoded;

            try
            {
                var bytes = Convert.FromBase64String(base64Credentials);
                decoded = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return Results.Unauthorized();
            }

            var parts = decoded.Split(':', 2);
            if (parts.Length != 2)
                return Results.Unauthorized();

            var clientId = parts[0];
            var clientSecret = parts[1];

            var token = await authService.AuthenticateAndGenerateTokenAsync(clientId, clientSecret);
            if (token == null)
                return Results.Unauthorized();

            return Results.Ok(new { access_token = token });
        });

        app.MapPost("/auth/login", [Authorize] async (UserAuthViewModel request, IAuthService authService, IUserService userService, ClaimsPrincipal user) =>
        {

            Claim? clientId = user.FindFirst("client_id");
            
            if (clientId == null || string.IsNullOrWhiteSpace(clientId.Value))
                return Results.Unauthorized();

            var userToAuth = await userService.FindUserByNameAndClientId(request.Username, clientId.Value);
            if (userToAuth == null)
                return Results.Unauthorized();

            string? token = await authService.AuthenticateUserAsync(userToAuth, request.Password);

            if (token == null)
                return Results.Unauthorized();

            return Results.Ok(new { access_token = token });
        });

        return app;
    }

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/users", [Authorize] async (UserViewModel request, IUserService userService, ClaimsPrincipal user) =>
        {
            Claim? clientId = user.FindFirst("client_id");

            Console.WriteLine($"Client ID: {clientId?.Value}");
            
            if (clientId == null || string.IsNullOrWhiteSpace(clientId.Value))
                return Results.Unauthorized();

        
            var result = await userService.CreateUserAsync(request, clientId.Value);
            
            return Results.Created($"/users/{result.Id}", new { result.Id, result.Username });
        });

        return app;
    }
}
