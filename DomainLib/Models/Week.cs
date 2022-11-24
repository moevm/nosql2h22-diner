using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public class Week: BaseModel
{
    #region Plain

    public int Monday { get; set; }
    public int Tuesday { get; set; }
    public int Wednesday { get; set; }
    public int Thursday { get; set; }
    public int Friday { get; set; }
    public int Saturday { get; set; }
    public int Sunday { get; set; }

    #endregion
    
    #region Plain

    [BsonRepresentation(BsonType.ObjectId)]
    public string ShiftId { get; set; } = null!;

    [BsonIgnore]
    public Shift Shift { get; set; } = null!;
    
    #endregion
    
}