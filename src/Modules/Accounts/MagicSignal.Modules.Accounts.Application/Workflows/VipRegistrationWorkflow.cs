using WorkflowCore.Interface;
using WorkflowCore.Models;
using MagicSignal.Modules.Accounts.Application.Workflows.Steps;

namespace MagicSignal.Modules.Accounts.Application.Workflows
{
    public class VipRegistrationWorkflow : IWorkflow<VipWorkflowData>
    {
        public string Id => "vip-registration-workflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<VipWorkflowData> builder)
        {
            builder
                .StartWith<CheckPaymentStep>()
                .Name("Check Payment")
                .If(data => data.PaymentStatus == "Valid").Do(then => 
                    then.Then<ProcessPaymentStep>()
                        .Name("Process Payment")
                        .If(data => data.PaymentCompleted).Do(success =>
                            success.Then<SendToAdminStep>()
                                .Name("Send to Admin")
                                .Then<WaitForApprovalStep>()
                                .Name("Wait for Admin Approval")
                                .If(data => data.AdminApproved).Do(approved =>
                                    approved.Then<ActivateVipStep>()
                                        .Name("Activate VIP")
                                        .Then<SendNotificationStep>()
                                        .Name("Send Success Notification")
                                )
                        )
                );
        }
    }
}