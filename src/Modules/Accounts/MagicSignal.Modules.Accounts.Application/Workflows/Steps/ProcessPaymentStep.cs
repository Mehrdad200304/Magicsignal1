using WorkflowCore.Models;
using WorkflowCore.Interface;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class ProcessPaymentStep : BaseWorkflowStep
    {
        protected override ExecutionResult ExecuteStep(IStepExecutionContext context)
        {
            var data = (VipWorkflowData)context.Workflow.Data;
            
            // شبیه‌سازی پردازش پرداخت
            // در واقعیت اینجا باید درخواست به درگاه پرداخت بفرستی
            
            // تولید TransactionId
            data.TransactionId = $"TXN_{DateTime.Now:yyyyMMddHHmmss}_{data.UserId}";
            
            // شبیه‌سازی موفقیت پرداخت (90% موفق)
            var random = new Random();
            var success = random.Next(1, 11) <= 9; // 90% chance
            
            if (success)
            {
                data.PaymentCompleted = true;
                data.PaymentStatus = "Completed";
                data.CurrentStep = "PaymentProcessed";
                
                return ExecutionResult.Next();
            }
            else
            {
                data.PaymentCompleted = false;
                data.PaymentStatus = "Failed";
                data.CurrentStep = "PaymentFailed";
                
                return ExecutionResult.Outcome("PaymentFailed");
            }
        }
    }
}