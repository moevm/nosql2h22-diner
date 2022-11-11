namespace DomainLib.DTO; 

public class DishDto {
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }
    public List<DishResourceDto> ListDishResourceDtos { get; set; } = new();
}