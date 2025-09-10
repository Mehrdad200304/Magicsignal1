using WorkflowCore.Models;
using WorkflowCore.Interface;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class SendToAdminStep : BaseWorkflowStep
    {
        protected override ExecutionResult ExecuteStep(IStepExecutionContext context)
        {
            var data = (VipWorkflowData)context.Workflow.Data;
            
            // شبیه‌سازی ارسال اطلاع‌رسانی به ادمین
            // در واقعیت اینجا باید:
            // 1. ایمیل به ادمین بفرستی
            // 2. نوتیفیکیشن در پنل ادمین ایجاد کنی
            // 3. SMS یا پیام در سیستم چت بفرستی
            
            try
            {
                // شبیه‌سازی ارسال ایمیل/نوتیف
                Console.WriteLine($"Sending VIP request to Admin: UserId={data.UserId}, Amount={data.Amount}");
                
                // تنظیم وضعیت
                data.AdminApprovalPending = true;
                data.CurrentStep = "WaitingForAdminApproval";
                
                // در اینجا می‌تونی لاگ یا اطلاع‌رسانی واقعی بفرستی
                
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                data.CurrentStep = "AdminNotificationFailed";
                return ExecutionResult.Outcome($"NotificationFailed: {ex.Message}");
            }
        }
    }
}