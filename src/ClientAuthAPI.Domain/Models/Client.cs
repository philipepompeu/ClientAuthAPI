namespace ClientAuthAPI.Domain.Models;

public class Client{    
    public string? Id { get; set; }
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    
}