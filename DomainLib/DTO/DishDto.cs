namespace DomainLib.DTO; 

public class DishDto {
    
    /// <summary>
    /// ID
    /// </summary>
    public string? Id { get; set; } = null!;
    
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Список ресурсов
    /// </summary>
    public List<DishResourceDto> ListDishResourceDtos { get; set; } = new();
}