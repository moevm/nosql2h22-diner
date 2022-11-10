using System.Text.Json.Serialization;
using DomainLib.Models;

namespace DomainLib.DTO;

public class UserDto
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("login")]
    public string Login { get; set; } = String.Empty;
    [JsonPropertyName("password")]
    public string Password { get; set; } = String.Empty;
    [JsonPropertyName("role")]
    public UserRole Role { get; set; } = UserRole.Waiter;
}