using System.Text.Json.Serialization;

namespace DomainLib.DTO;

public class GetShiftDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("dayOfWeek")]
    public DayOfWeek? DayOfWeek { get; set; }
    
    [JsonPropertyName("hours")]
    public int Hours { get; set; }
}