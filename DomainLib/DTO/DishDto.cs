using System.Text.Json.Serialization;
using DomainLib.Models;

namespace DomainLib.DTO; 

public class DishDto {
    
    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; } = null!;
    
    /// <summary>
    /// Название
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    /// <summary>
    /// Описание
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    /// <summary>
    /// Цена
    /// </summary>
    [JsonPropertyName("price")]
    public int Price { get; set; }
    
    [JsonPropertyName("dishType")]
    public DishType DishType { get; set; }
    /// <summary>
    /// Список ресурсов
    /// </summary>
    [JsonPropertyName("listDishResourceDtos")]
    public List<DishResourceDto> ListDishResourceDtos { get; set; } = new();
}