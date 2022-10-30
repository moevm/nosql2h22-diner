namespace DomainLib.Models;

public enum UnitType
{
    
}

public class DishResource: BaseModel
{
    public int Amount { get; set; }
    public UnitType Unit { get; set; }
}