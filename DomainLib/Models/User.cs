using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

/// <summary>
/// 
/// </summary>
public class User: BaseModel
{
    /// <summary>
    /// 
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    public string? SecondName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string LastName { get; set; } = string.Empty;
}