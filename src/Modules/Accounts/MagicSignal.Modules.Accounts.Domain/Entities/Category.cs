using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MagicSignal.Modules.Accounts.Domain.Entities
{
    public class Category
    {
        [Key] 
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "نام دسته‌بندی الزامی است")]
        [MaxLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیش از ۱۰۰ کاراکتر باشد")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "توضیحات نمی‌تواند بیش از ۵۰۰ کاراکتر باشد")]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation property - یک Category می‌تونه چندتا Article داشته باشه
        [JsonIgnore] // ← این خط اضافه شد
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}