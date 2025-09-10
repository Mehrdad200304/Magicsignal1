using WorkflowCore.Models;
using WorkflowCore.Interface;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class ActivateVipStep : BaseWorkflowStep
    {
        protected override ExecutionResult ExecuteStep(IStepExecutionContext context)
        {
            var data = (VipWorkflowData)context.Workflow.Data;
            
            try
            {
                // محاسبه تاریخ انقضا بر اساس نوع پکیج
                DateTime expirationDate;
                if (data.PackageType.ToLower() == "monthly")
                {
                    expirationDate = DateTime.Now.AddMonths(1);
                }
                else if (data.PackageType.ToLower() == "yearly")
                {
                    expirationDate = DateTime.Now.AddYears(1);
                }
                else
                {
                    expirationDate = DateTime.Now.AddMonths(1); // پیش‌فرض
                }
                
                // فعال‌سازی VIP
                data.VipActivated = true;
                data.ExpirationDate = expirationDate;
                data.CurrentStep = "VipActivated";
                
                // اینجا باید در دیتابیس کاربر رو VIP کنی
                Console.WriteLine($"VIP Activated for User {data.UserId} until {expirationDate:yyyy-MM-dd}");
                
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                data.CurrentStep = "VipActivationFailed";
                return ExecutionResult.Outcome($"ActivationFailed: {ex.Message}");
            }
        }
    }
}