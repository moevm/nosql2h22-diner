using System.Text.Json.Serialization;

namespace DomainLib.DTO;

public class GetShiftDto
{
    [JsonPropertyName("dayOfWeek")]
    public DayOfWeek? DayOfWeek { get; set; }
    
    [JsonPropertyName("hours")]
    public int Hours { get; set; }
}