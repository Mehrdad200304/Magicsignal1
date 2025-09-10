namespace MagicSignal.Modules.Accounts.Application.DTOs.Workflow
{
    public class VipRegistrationRequestDto
    {
        public int UserId { get; set; }
        public string PackageType { get; set; } // "Monthly" یا "Yearly"
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // "Card", "Bank", etc
    }
}