namespace DomainLib.DTO; 

public class DishResourceDto {
    public string? Id { get; set; } = null!;
    public string ResourceId { get; set; } = null!;
    public int Required { get; set; }
}