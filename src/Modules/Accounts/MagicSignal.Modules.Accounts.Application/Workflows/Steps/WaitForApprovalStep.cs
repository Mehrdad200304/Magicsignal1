using WorkflowCore.Models;
using WorkflowCore.Interface;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class WaitForApprovalStep : BaseWorkflowStep
    {
        protected override ExecutionResult ExecuteStep(IStepExecutionContext context)
        {
            var data = (VipWorkflowData)context.Workflow.Data;
            
            // این step منتظر می‌مونه تا ادمین تصمیم بگیره
            // WorkflowCore خودش این انتظار رو مدیریت می‌کنه
            
            if (data.AdminApproved)
            {
                data.AdminApprovalPending = false;
                data.CurrentStep = "AdminApproved";
                
                return ExecutionResult.Next();
            }
            else if (!string.IsNullOrEmpty(data.AdminComment) && data.AdminComment.Contains("Rejected"))
            {
                data.AdminApprovalPending = false;
                data.CurrentStep = "AdminRejected";
                
                return ExecutionResult.Outcome("AdminRejected");
            }
            else
            {
                // هنوز در انتظار تصمیم ادمین
                data.CurrentStep = "WaitingForAdminApproval";
                
                // WorkflowCore رو مجبور می‌کنه که این step رو دوباره چک کنه
                return ExecutionResult.Persist(data);
            }
        }
    }
}