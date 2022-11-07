using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainLib.Models;

public enum PaymentStatus
{
    Waiting,
    Approval,
    Rejected
}

public enum PaymentType
{
    ForOrder,
    Lease
}

public class Payment: BaseModel
{
    #region Plain

    public PaymentStatus Status { get; set; }
    public PaymentType Type { get; set; }
    public int NumericId { get; set; }
    public int Price { get; set; }

    #endregion

    #region Relations

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonIgnore]
    public User User { get; set; } = null!;

    #endregion
}