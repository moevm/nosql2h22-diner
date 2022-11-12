using DomainLib.Models;

namespace DomainLib.DTO; 

public class ResourceDto {
    /// <summary>
    /// Рессурс
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Количество
    /// </summary>
    public int Amount { get; set; }
    public Unit Unit { get; set; }
}