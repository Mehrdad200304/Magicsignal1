namespace MagicSignal.Modules.Accounts.Application.DTOs.Workflow
{
    public class VipRegistrationResponseDto
    {
        public bool Success { get; set; } // آیا عملیات موفق بود یا نه
        public string Message { get; set; } // پیام نتیجه (موفقیت یا خطا)
        public int? RequestId { get; set; } // شناسه درخواست VIP (برای ردیابی)
        public DateTime? ExpirationDate { get; set; } // تاریخ پایان عضویت VIP
        public string TransactionId { get; set; } // کد تراکنش یا شماره پیگیری
    }
}