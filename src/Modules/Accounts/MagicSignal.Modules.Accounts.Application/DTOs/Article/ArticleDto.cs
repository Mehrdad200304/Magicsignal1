using System.ComponentModel.DataAnnotations;

namespace MagicSignal.Modules.Accounts.Application.DTOs.Article
{
    public class ArticleDto
    {
        [Required(ErrorMessage = "عنوان مقاله الزامی است")]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "محتوای مقاله الزامی است")]
        [MinLength(10)]
        public string Content { get; set; }

        public string? Summary { get; set; }

        [Required] public Guid AuthorId { get; set; }

        [Required] public Guid CategoryId { get; set; }

        public bool IsPublished { get; set; } = false;
    }
}