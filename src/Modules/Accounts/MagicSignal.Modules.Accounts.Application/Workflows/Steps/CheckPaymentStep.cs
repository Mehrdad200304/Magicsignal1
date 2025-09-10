using WorkflowCore.Models;
using WorkflowCore.Interface;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class CheckPaymentStep : BaseWorkflowStep
    {
        protected override ExecutionResult ExecuteStep(IStepExecutionContext context)
        {
            var data = (VipWorkflowData)context.Workflow.Data;
            
            // شبیه‌سازی چک کردن پرداخت
            // در واقعیت اینجا باید با API بانک یا درگاه پرداخت چک کنی
            
            if (data.Amount > 0 && !string.IsNullOrEmpty(data.PaymentMethod))
            {
                data.PaymentStatus = "Valid";
                data.CurrentStep = "PaymentChecked";
                
                return ExecutionResult.Next();
            }
            else
            {
                data.PaymentStatus = "Invalid";
                data.CurrentStep = "PaymentFailed";
                
                return ExecutionResult.Outcome("PaymentInvalid");
            }
        }
    }
}