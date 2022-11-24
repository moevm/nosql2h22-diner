using System.Text.Json.Serialization;

namespace DomainLib.DTO;

public class AuthDto
{
    /// <summary>
    /// Логин
    /// </summary>
    [JsonPropertyName("login")] public string Login { get; set; } = String.Empty;

    /// <summary>
    /// Пароль
    /// </summary>
    [JsonPropertyName("password")] public string Password { get; set; } = String.Empty;
}