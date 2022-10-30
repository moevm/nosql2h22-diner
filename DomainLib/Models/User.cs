using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public enum UserRole
{
    
}

public enum UserStatus
{
    
}

/// <summary>
/// 
/// </summary>
public class User: BaseModel
{
    /// <summary>
    /// 
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Login { get; set; } = null!;
    
    /// <summary>
    /// 
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserStatus Status { get; set; }
}