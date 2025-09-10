namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class VipWorkflowData
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }
        public string PackageType { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }

        // Payment Status
        public bool PaymentCompleted { get; set; }
        public string PaymentStatus { get; set; }

        // Admin Approval
        public bool AdminApprovalPending { get; set; }
        public bool AdminApproved { get; set; }
        public string AdminComment { get; set; }

        // VIP Activation
        public bool VipActivated { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Workflow Status
        public string CurrentStep { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}