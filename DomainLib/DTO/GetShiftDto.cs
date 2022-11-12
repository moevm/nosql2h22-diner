using System.Text.Json.Serialization;

namespace DomainLib.DTO;

public class GetShiftDto
{
    /// <summary>
    /// День недели (знаения от 0 до 6, 0 - воскресенье)
    /// </summary>
    [JsonPropertyName("dayOfWeek")]
    public DayOfWeek? DayOfWeek { get; set; }
    
    /// <summary>
    /// Маска для часов
    /// </summary>
    [JsonPropertyName("hours")]
    public int Hours { get; set; }

    /// <summary>
    /// Свободен в эти часы или нет
    /// </summary>
    [JsonPropertyName("free")]
    public bool Free { get; set; }
}