using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    /// <summary>
    /// Step برای ارسال اطلاع‌رسانی به کاربر بعد از تصمیم ادمین
    /// </summary>
    public class NotificationStep : StepBody
    {
        // ورودی ها
        public Guid UserId { get; set; }
        public Guid VipRequestId { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminComment { get; set; }
        public string? UserEmail { get; set; }
        public string? Username { get; set; }
        
        // خروجی ها
        public bool NotificationSent { get; set; }
        public List<string> SentNotifications { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public DateTime ProcessedAt { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            try
            {
                Console.WriteLine($"NotificationStep: Sending notification to user {UserId} for VIP request {VipRequestId}");
                
                ProcessedAt = DateTime.Now;
                
                if (IsApproved)
                {
                    // ✅ درخواست تأیید شده - اطلاع‌رسانی مثبت
                    SendApprovalNotification();
                }
                else
                {
                    // ❌ درخواست رد شده - اطلاع‌رسانی منفی
                    SendRejectionNotification();
                }
                
                NotificationSent = true;
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in NotificationStep: {ex.Message}");
                
                NotificationSent = false;
                ErrorMessage = ex.Message;
                
                // حتی اگه notification fail بشه، workflow رو ادامه می‌دیم
                return ExecutionResult.Next();
            }
        }
        
        /// <summary>
        /// ارسال اطلاع‌رسانی برای تأیید درخواست
        /// </summary>
        private void SendApprovalNotification()
        {
            try
            {
                // 📱 In-App Notification (فعلاً Console Log)
                var inAppMessage = $"🎉 تبریک {Username}! درخواست VIP شما تأیید شد و اکنون دسترسی VIP دارید.";
                Console.WriteLine($"📱 In-App Notification: {inAppMessage}");
                SentNotifications.Add("In-App");
                
                // 📧 Email Notification (فعلاً Mock)
                if (!string.IsNullOrEmpty(UserEmail))
                {
                    var emailSubject = "تأیید درخواست VIP";
                    var emailBody = $@"
سلام {Username}،

خبر خوش! درخواست VIP شما با موفقیت تأیید شد.

🎉 مزایای VIP شما:
• دسترسی به اشتراک ویژه
• اولویت در پشتیبانی
• محتوای اختصاصی

تاریخ فعال‌سازی: {DateTime.Now:yyyy/MM/dd}
اعتبار تا: {DateTime.Now.AddMonths(1):yyyy/MM/dd}

{(string.IsNullOrEmpty(AdminComment) ? "" : $"نظر ادمین: {AdminComment}")}

با تشکر،
تیم MagicSignal
                    ";
                    
                    Console.WriteLine($"📧 Email Sent to {UserEmail}:");
                    Console.WriteLine($"   Subject: {emailSubject}");
                    Console.WriteLine($"   Body: {emailBody}");
                    
                    // فعلاً فیک - بعداً با EmailService جایگزین می‌شه
                    // await _emailService.SendEmailAsync(UserEmail, emailSubject, emailBody);
                    
                    SentNotifications.Add("Email");
                }
                
                // 📲 SMS Notification (اختیاری - فعلاً Mock)
                var smsMessage = $"تبریک {Username}! درخواست VIP شما تأیید شد. از این پس دسترسی VIP دارید. MagicSignal";

 
                Console.WriteLine($"📲 SMS: {smsMessage}");
                
                // فعلاً فیک
                // await _smsService.SendSmsAsync(userPhoneNumber, smsMessage);
                
                SentNotifications.Add("SMS");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error sending approval notifications: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// ارسال اطلاع‌رسانی برای رد درخواست
        /// </summary>
        private void SendRejectionNotification()
        {
            try
            {
                // 📱 In-App Notification
                var inAppMessage = $"متأسفانه {Username}، درخواست VIP شما رد شد.";
                Console.WriteLine($"📱 In-App Notification: {inAppMessage}");
                SentNotifications.Add("In-App");
                
                // 📧 Email Notification
                if (!string.IsNullOrEmpty(UserEmail))
                {
                    var emailSubject = "پاسخ درخواست VIP";
                    var emailBody = $@"
سلام                    {Username}،

متأسفانه درخواست VIP شما در تاریخ                    {DateTime.Now:yyyy/MM/dd} رد شد.

                    {(string.IsNullOrEmpty(AdminComment) ? "" : $"دلیل: {AdminComment}")}

💡 راه‌های بهبود:
• مطالعه قوانین و شرایط VIP
• فعالیت بیشتر در پلتفرم
• رعایت اصول و آداب

می‌توانید بعداً مجدداً درخواست دهید.

با تشکر،
تیم MagicSignal
                    ";
                    
                    Console.WriteLine($"📧 Email Sent to {UserEmail}:");
                    Console.WriteLine($"   Subject: {emailSubject}");
                    Console.WriteLine($"   Body: {emailBody}");
                    
                    SentNotifications.Add("Email");
                }
                
                // 📲 SMS Notification (کوتاه‌تر)
                var smsMessage = $"{Username}، متأسفانه درخواست VIP شما رد شد. جهت اطلاعات بیشتر به ایمیل مراجعه کنید. MagicSignal";
                Console.WriteLine($"📲 SMS: {smsMessage}");
                
                SentNotifications.Add("SMS");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error sending rejection notifications: {ex.Message}");
                throw;
            }
        }
    }
    
    /// <summary>
    /// نتیجه ارسال اطلاع‌رسانی
    /// </summary>
    public class NotificationResult
    {
        public bool Success { get; set; }
        public List<string> SentChannels { get; set; } = new();
        public List<string> FailedChannels { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public DateTime SentAt { get; set; }
        
        public static NotificationResult CreateSuccess(List<string> sentChannels)
        {
            return new NotificationResult
            {
                Success = true,
                SentChannels = sentChannels,
                SentAt = DateTime.Now
            };
        }
        
        public static NotificationResult CreateFailure(string error, List<string>? failedChannels = null)
        {
            return new NotificationResult
            {
                Success = false,
                ErrorMessage = error,
                FailedChannels = failedChannels ?? new List<string>(),
                SentAt = DateTime.Now
            };
        }
    }
}