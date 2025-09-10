using WorkflowCore.Interface;
using WorkflowCore.Models;
using MagicSignal.Modules.Accounts.Application.Workflows.Steps;

namespace MagicSignal.Modules.Accounts.Application.Workflows
{
    public class VipApprovalWorkflow : IWorkflow<VipWorkflowData>
    {
        public string Id => "VipApprovalWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<VipWorkflowData> builder)
        {
            builder
                .StartWith<AdminApprovalStep>()
                .Input(step => step.VipRequestId, data => data.VipRequestId)
                .Input(step => step.RequestorEmail, data => data.UserEmail)
                .Input(step => step.RequestorUsername, data => data.Username)
                .Input(step => step.RequestDate, data => data.RequestDate);
            // .Then<UpdateVipStatusStep>()
            //     .Input(step => step.VipRequestId, data => data.VipRequestId)
            //     .Input(step => step.UserId, data => data.UserId)
            //     .Input(step => step.IsApproved, data => data.IsApproved)
            //     .Input(step => step.AdminComment, data => data.AdminComment)
            //     .Input(step => step.AdminUserId, data => data.AdminUserId)
            // .Then<NotificationStep>()
            //     .Input(step => step.VipRequestId, data => data.VipRequestId)
            //     .Input(step => step.UserId, data => data.UserId)
            //     .Input(step => step.IsApproved, data => data.IsApproved)
            //     .Input(step => step.AdminComment, data => data.AdminComment)
            //     .Input(step => step.UserEmail, data => data.UserEmail)
            //     .Input(step => step.Username, data => data.Username);
        }
    }
    
    /// <summary>
    /// Data class برای VIP Workflow
    /// </summary>
    public class VipWorkflowData
    {
        public Guid VipRequestId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        
        // نتیجه تصمیم ادمین
        public bool IsApproved { get; set; }
        public string? AdminComment { get; set; }
        public Guid AdminUserId { get; set; }
        public DateTime? DecisionDate { get; set; }
        // فیلدهای اضافه برای VipRegistrationWorkflow
        public string PaymentStatus { get; set; } = string.Empty;
        public bool PaymentCompleted { get; set; }
        public bool AdminApproved { get; set; }
        
        // فیلدهای VipWorkflowController
        public string PackageType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CurrentStep { get; set; } = string.Empty;
    }
}