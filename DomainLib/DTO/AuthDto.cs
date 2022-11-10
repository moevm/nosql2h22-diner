using System.Text.Json.Serialization;

namespace DomainLib.DTO;

public class AuthDto
{
    [JsonPropertyName("login")] public string Login { get; set; } = String.Empty;

    [JsonPropertyName("password")] public string Password { get; set; } = String.Empty;
}