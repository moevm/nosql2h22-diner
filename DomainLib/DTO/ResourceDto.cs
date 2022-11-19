using DomainLib.Models;

namespace DomainLib.DTO; 

public class ResourceDto {
    
    public string? Id { get; set; } = null!;
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