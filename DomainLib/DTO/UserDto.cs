using System.Text.Json.Serialization;
using DomainLib.Models;

namespace DomainLib.DTO;

public class UserDto
{
    /// <summary>
    /// Полное имя
    /// </summary>
    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Логин
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; } = String.Empty;
    /// <summary>
    /// Пароль
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; } = String.Empty;
    /// <summary>
    /// Роль
    /// </summary>
    [JsonPropertyName("role")]
    public UserRole Role { get; set; } = UserRole.Waiter;
}