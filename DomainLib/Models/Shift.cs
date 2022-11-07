using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class Shift: BaseModel
{
    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string TargetWeekId { get; set; } = null!;
    
    [BsonIgnore]
    public Week TargetWeek { get; set; } = null!;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Weeks { get; set; } = null!;
    
    [BsonIgnore]
    public List<Week> WeeksList { get; set; } = null!;

    #endregion
}