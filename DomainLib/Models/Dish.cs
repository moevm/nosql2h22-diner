using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishType
{
    [EnumMember(Value = "Soup")]
    Soup,
    
    [EnumMember(Value = "Snack")]
    Snack,
    
    [EnumMember(Value = "Bar")]
    Bar,
    
    [EnumMember(Value = "Hot")]
    Hot,
}


public class Dish: BaseModel
{
    #region Plain

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }

    public DishType DishType { get; set; }

    #endregion

    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Comments { get; set; } = null!;

    [BsonIgnore]
    public List<Comment> CommentsList { get; set; } = null!;

    #endregion
}