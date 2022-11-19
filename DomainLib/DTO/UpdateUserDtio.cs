using System.Text.Json.Serialization;
using DomainLib.Models;

namespace DomainLib.DTO;

public class UpdateUserDto 
{
    /// <summary>
    /// Пароль
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = String.Empty;  
    /// <summary>
    /// Полное имя
    /// </summary>
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; } = string.Empty;
    /// <summary>
    /// Логин
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; set; } = String.Empty;
    /// <summary>
    /// Роль
    /// </summary>
    [JsonPropertyName("role")]
    public UserRole? Role { get; set; } = UserRole.Waiter;
    
    [JsonPropertyName("status")]
    public UserStatus? Status { get; set; }
}