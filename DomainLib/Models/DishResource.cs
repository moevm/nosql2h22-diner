using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class DishResource: BaseModel
{
    #region Plain

    public int Required { get; set; }

    #endregion

    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public string DishId { get; set; } = null!;

    [BsonIgnore]
    public Dish Dish { get; set; } = null!;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string ResourceId { get; set; } = null!;

    [BsonIgnore]
    public Resource Resource { get; set; } = null!;

    #endregion
}