using System.ComponentModel.DataAnnotations;

namespace MagicSignal.Modules.Accounts.Application.DTOs.Article
{
    public class UpdateArticleDto
    {
        [Required]
        public Guid Id { get; set; }
        
        [StringLength(200, MinimumLength = 5)]
        public string? Title { get; set; }
        
        public string? Content { get; set; }
        public string? Summary { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? IsPublished { get; set; }
    }
}