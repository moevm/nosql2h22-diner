using System.Text.Json.Serialization;
using DomainLib.Models;

namespace DomainLib.DTO; 

public class ResourceDto {
    [JsonPropertyName("id")]
    public string? Id { get; set; } = null!;
    
    /// <summary>
    /// Рессурс
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Количество
    /// </summary>
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("unit")]
    public Unit Unit { get; set; }
}