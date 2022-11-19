using System.Text.Json.Serialization;
using DomainLib.Models;

namespace DomainLib.DTO;

public class PaymentDto
{
    [JsonPropertyName("status")]
    public PaymentStatus Status { get; set; }
    [JsonPropertyName("type")]
    public PaymentType Type { get; set; }
    [JsonPropertyName("price")]
    public int Price { get; set; }
    
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;
}