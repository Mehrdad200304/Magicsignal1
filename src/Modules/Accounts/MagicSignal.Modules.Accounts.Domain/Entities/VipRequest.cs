namespace MagicSignal.Modules.Accounts.Domain.Entities;

public class VipRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string RequestType { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public Guid? ApprovedBy { get; set; }
    public User? User { get; set; }
}