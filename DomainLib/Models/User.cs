using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    [EnumMember(Value = "Admin")]
    Admin,
    
    [EnumMember(Value = "Waiter")]
    Waiter,
    
    [EnumMember(Value = "Manager")]
    Manager,
    
    [EnumMember(Value = "Cook")]
    Cook,
    
    [EnumMember(Value = "Steward")]
    Steward
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserStatus
{
    [EnumMember(Value = "InWork")]
    InWork,
    
    [EnumMember(Value = "NotInWork")]
    NotInWork,
    
    [EnumMember(Value = "Vacation")]
    Vacation,
    
    [EnumMember(Value = "Blocked")]
    Blocked
}

public class User: BaseModel
{
    #region Plain
    
    public string FullName { get; set; } = null!;
    public string Login { get; set; } = null!;
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