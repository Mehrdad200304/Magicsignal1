using WorkflowCore.Models;
using WorkflowCore.Interface;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public class SendNotificationStep : BaseWorkflowStep
    {
        protected override ExecutionResult ExecuteStep(IStepExecutionContext context)
        {
            var data = (VipWorkflowData)context.Workflow.Data;
            
            try
            {
                // ارسال پیام موفقیت به کاربر
                var message = $"تبریک! عضویت VIP شما فعال شد.\n" +
                              $"نوع پکیج: {data.PackageType}\n" +
                              $"تاریخ انقضا: {data.ExpirationDate:yyyy-MM-dd}\n" +
                              $"کد پیگیری: {data.TransactionId}";
                
                // شبیه‌سازی ارسال ایمیل/SMS/پوش نوتیفیکیشن
                Console.WriteLine($"Notification sent to User {data.UserId}: {message}");
                
                data.CurrentStep = "Completed";
                
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                data.CurrentStep = "NotificationFailed";
                // حتی اگه نوتیف فیل شد، VIP فعال شده پس ادامه بده
                return ExecutionResult.Next();
            }
        }
    }
}