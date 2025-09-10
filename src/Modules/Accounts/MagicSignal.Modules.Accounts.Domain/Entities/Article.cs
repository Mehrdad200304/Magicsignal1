using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicSignal.Modules.Accounts.Domain.Entities
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "عنوان مقاله الزامی است")]
        [MaxLength(200, ErrorMessage = "عنوان نمی‌تواند بیش از ۲۰۰ کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "محتوای مقاله الزامی است")]
        [MinLength(10, ErrorMessage = "محتوا باید حداقل ۱۰ کاراکتر باشد")]
        [MaxLength(50000, ErrorMessage = "محتوا نمی‌تواند بیش از ۵۰۰۰۰ کاراکتر باشد")]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "خلاصه نمی‌تواند بیش از ۵۰۰ کاراکتر باشد")]
        public string? Summary { get; set; }

        // Foreign Keys
        [Required(ErrorMessage = "شناسه نویسنده الزامی است")]
        public Guid AuthorId { get; set; }

        [Required(ErrorMessage = "شناسه دسته‌بندی الزامی است")]
        public Guid CategoryId { get; set; }

        // Dates
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // States
        public bool IsPublished { get; set; } = false;
        public int ViewCount { get; set; } = 0;

        // Navigation Properties
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;
    }
}