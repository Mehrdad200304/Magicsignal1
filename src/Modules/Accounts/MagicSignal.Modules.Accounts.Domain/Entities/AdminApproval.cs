using System.ComponentModel.DataAnnotations;

namespace MagicSignal.Modules.Accounts.Domain.Entities
{
    public class AdminApproval
    {
        [Key]
        public Guid Id { get; set; }
        
        // شناسه درخواست VIP
        [Required]
        public Guid VipRequestId { get; set; }
        
        // شناسه ادمین که تصمیم گرفته
        [Required]
        public Guid AdminUserId { get; set; }
        
        // نوع تصمیم (تأیید/رد)
        public bool IsApproved { get; set; }
        
        // کامنت ادمین
        [MaxLength(500)]
        public string? AdminComment { get; set; }
        
        // تاریخ تصمیم‌گیری
        [Required]
        public DateTime DecisionDate { get; set; }
        
        // تاریخ ایجاد رکورد
        [Required]
        public DateTime CreatedAt { get; set; }
        
        // آخرین آپدیت
        public DateTime? UpdatedAt { get; set; }
        
        // وضعیت فعال/غیرفعال
        public bool IsActive { get; set; }
        
        // Navigation Properties - روابط با Entity های دیگر
        
        // رابطه با درخواست VIP (اگه VipRequest Entity داری)
        // public VipRequest VipRequest { get; set; }
        
        // رابطه با User (ادمین)
        // public User AdminUser { get; set; }
        
        // Constructor
        public AdminApproval()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }
    }
}