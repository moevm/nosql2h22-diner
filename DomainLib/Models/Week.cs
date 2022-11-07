using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class Week: BaseModel
{
    #region Plain

    public string Monday { get; set; } = null!;
    public string Tuesday { get; set; } = null!;
    public string Wednesday { get; set; } = null!;
    public string Thursday { get; set; } = null!;
    public string Friday { get; set; } = null!;
    public string Saturday { get; set; } = null!;
    public string Sunday { get; set; } = null!;

    #endregion
    
    #region Plain

    [BsonRepresentation(BsonType.ObjectId)]
    public string ShiftId { get; set; } = null!;

    [BsonIgnore]
    public Shift Shift { get; set; } = null!;
    
    #endregion
    
}