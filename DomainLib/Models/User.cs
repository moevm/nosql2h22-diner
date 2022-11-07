using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public enum UserRole
{
    Admin,
    Waiter,
    Manager,
    Cook,
    Steward
}

public enum UserStatus
{
    InWork,
    NotInWork,
    Vacation,
    Blocked
}

public class User: BaseModel
{
    #region Plain
    
    public string FullName { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }

    #endregion

    #region Relations
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string ShiftId { get; set; } = null!;
    
    [BsonIgnore]
    public Shift Shift { get; set; } = null!;

    #endregion

}