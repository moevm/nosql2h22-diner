namespace DomainLib.DTO;

public class СommentDto
{
    public string? Id { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? DishId { get; set; } = null!;
    public string? ResourceId { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
}