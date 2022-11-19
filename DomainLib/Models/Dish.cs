using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class Dish: BaseModel
{
    #region Plain

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }

    #endregion

    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Comments { get; set; } = null!;

    [BsonIgnore]
    public List<Comment> CommentsList { get; set; } = null!;

    #endregion
}