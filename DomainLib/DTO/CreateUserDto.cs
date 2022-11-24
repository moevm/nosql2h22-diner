using System.Text.Json.Serialization;

namespace DomainLib.DTO;

public class CreateUserDto: UserDto
{
    /// <summary>
    /// Пароль
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; } = String.Empty;  
}