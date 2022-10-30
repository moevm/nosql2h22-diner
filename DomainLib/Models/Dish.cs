namespace DomainLib.Models;

public class Dish: BaseModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
}