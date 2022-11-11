using DomainLib.Models;

namespace DomainLib.DTO; 

public class ResourceDto {
    public string Name { get; set; } = null!;
    public int Amount { get; set; }
    public Unit Unit { get; set; }
}