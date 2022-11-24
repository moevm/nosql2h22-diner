using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Unit
{
    [EnumMember(Value = "Kg")]
    Kg,
    
    [EnumMember(Value = "Liter")]
    Liter,
    
    [EnumMember(Value = "Items")]
    Items
}

public class Resource: BaseModel
{
    #region Plain

    public string Name { get; set; } = null!;
    public int Amount { get; set; }
    public Unit Unit { get; set; }

    #endregion

    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Comments { get; set; } = null!;

    [BsonIgnore]
    public List<Comment> CommentsList { get; set; } = null!;

    #endregion
}