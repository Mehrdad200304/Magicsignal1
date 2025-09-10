using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    /// <summary>
    /// Step برای آپدیت کردن وضعیت VIP کاربر بعد از تأیید ادمین
    /// </summary>
    public class UpdateVipStatusStep : StepBody
    {
        // ورودی ها
        public Guid VipRequestId { get; set; }
        public Guid UserId { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminComment { get; set; }
        public Guid AdminUserId { get; set; }
        
        // خروجی ها
        public bool UpdateSuccess { get; set; }
        public string? UpdateMessage { get; set; }
        public DateTime ProcessedAt { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            try
            {
                Console.WriteLine($"UpdateVipStatusStep: Processing VIP request {VipRequestId} for user {UserId}");
                
                ProcessedAt = DateTime.Now;
                
                if (IsApproved)
                {
                    // ✅ درخواست تأیید شده - کاربر VIP می‌شه
                    
                    // اینجا باید UserService رو صدا کنیم
                    // فعلاً Mock می‌کنیم
                    
                    Console.WriteLine($"✅ User {UserId} is now VIP! Request {VipRequestId} approved by admin {AdminUserId}");
                    
                    UpdateSuccess = true;
                    UpdateMessage = "کاربر با موفقیت VIP شد";
                    
                    // فعلاً فیکه - بعداً واقعی می‌شه
                    // await _userService.SetUserVipStatusAsync(UserId, true, DateTime.Now.AddMonths(1));
                }
                else
                {
                    // ❌ درخواست رد شده - کاربر عادی می‌مونه
                    
                    Console.WriteLine($"❌ VIP request {VipRequestId} for user {UserId} was rejected by admin {AdminUserId}");
                    Console.WriteLine($"Rejection reason: {AdminComment}");
                    
                    UpdateSuccess = true;
                    UpdateMessage = "درخواست رد شد - وضعیت کاربر تغییر نکرد";
                    
                    // هیچ تغییری در وضعیت کاربر نمی‌دیم
                }
                
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in UpdateVipStatusStep: {ex.Message}");
                
                UpdateSuccess = false;
                UpdateMessage = $"خطا در آپدیت وضعیت: {ex.Message}";
                
                // در صورت خطا، workflow رو fail نمی‌کنیم، ولی لاگ می‌کنیم
                return ExecutionResult.Next();
            }
        }
    }
    
    /// <summary>
    /// DTO برای نتیجه آپدیت وضعیت VIP
    /// </summary>
    public class VipUpdateResult
    {
        public Guid UserId { get; set; }
        public Guid VipRequestId { get; set; }
        public bool IsVipNow { get; set; }
        public DateTime? VipExpirationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
        
        public static VipUpdateResult Success(Guid userId, Guid vipRequestId, DateTime? expirationDate = null)
        {
            return new VipUpdateResult
            {
                UserId = userId,
                VipRequestId = vipRequestId,
                IsVipNow = true,
                VipExpirationDate = expirationDate ?? DateTime.Now.AddMonths(1),
                Status = "Success",
                Message = "وضعیت VIP با موفقیت فعال شد",
                ProcessedAt = DateTime.Now
            };
        }
        public static VipUpdateResult Rejected(Guid userId, Guid vipRequestId, string reason)
        {
            return new VipUpdateResult
            {
                UserId = userId,
                VipRequestId = vipRequestId,
                IsVipNow = false,
                VipExpirationDate = null,
                Status = "Rejected", 
                Message = $"درخواست رد شد: {reason}",
                ProcessedAt = DateTime.Now
            };
        }
        
        public static VipUpdateResult Error(Guid userId, Guid vipRequestId, string error)
        {
            return new VipUpdateResult
            {
                UserId = userId,
                VipRequestId = vipRequestId,
                IsVipNow = false,
                VipExpirationDate = null,
                Status = "Error",
                Message = $"خطا: {error}",
                ProcessedAt = DateTime.Now
            };
        }
    }
}