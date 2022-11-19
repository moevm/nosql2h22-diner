using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class AuthInfo: BaseModel
{
    #region Plain

    public string PasswordHash { get; set; } = string.Empty;

    #endregion


    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonIgnore]
    public User User { get; set; } = null!;
    
    #endregion
}