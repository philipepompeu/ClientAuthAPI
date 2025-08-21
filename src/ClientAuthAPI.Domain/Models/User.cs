namespace ClientAuthAPI.Domain.Models;

public class User
{    
    public string? Id { get; set; }
    public string Username { get; set; } = string.Empty;    
    public string PasswordHash { get; set; } = string.Empty;    
    public string ClientId { get; set; } = string.Empty;    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
