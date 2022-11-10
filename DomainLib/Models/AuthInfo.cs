using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class AuthInfo: BaseModel
{
    public string PasswordHash { get; set; } = string.Empty;

    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonIgnore]
    public User User { get; set; } = null!;
    
    #endregion
}