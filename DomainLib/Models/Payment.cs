namespace DomainLib.Models;

public enum PaymentStatus
{
    Waiting,
    Approval,
    Rejected
}

public class Payment: BaseModel
{
    public PaymentStatus Status { get; set; }
}