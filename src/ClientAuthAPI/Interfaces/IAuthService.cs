namespace ClientAuthAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAndGenerateTokenAsync(string clientId, string clientSecret);
    }
}