namespace MagicSignal.Modules.Accounts.Application.DTOs.Workflow
{
    public class AdminApprovalDto
    {
        public Guid RequestId { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminComment { get; set; }
    }
}