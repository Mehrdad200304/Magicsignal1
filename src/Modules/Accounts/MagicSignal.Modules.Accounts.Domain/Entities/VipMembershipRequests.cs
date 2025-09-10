using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicSignal.Modules.Accounts.Domain.Entities
{
    public class VipMembershipRequests
    {
        [Key]
        public Guid Id { get; set; }
    
        [Required]
        public Guid UserId { get; set; }
    
        [Required]
        [MaxLength(50)]
        public string RequestType { get; set; } = string.Empty;
    
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";
    
        public string? Description { get; set; }
    
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
        public DateTime? UpdatedAt { get; set; }
    
        public DateTime? ProcessedAt { get; set; }
    
        public Guid? ProcessedBy { get; set; }
        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    
        [ForeignKey("ProcessedBy")]
        public virtual User? ProcessedByUser { get; set; }
    }
}