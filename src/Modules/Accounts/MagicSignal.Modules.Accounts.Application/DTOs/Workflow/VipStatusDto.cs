namespace MagicSignal.Modules.Accounts.Application.DTOs.Workflow
{
    public class VipStatusDto
    {
        public Guid UserId { get; set; }
        public bool IsVip { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string PackageType { get; set; }
    }
}