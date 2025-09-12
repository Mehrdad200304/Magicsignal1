namespace MagicSignal.Modules.Accounts.Application.DTOs.Article;
public class ArticleResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedDate { get; set; }
    public int ViewCount { get; set; }
    public bool IsPublished { get; set; }
    public string AuthorName { get; set; }
    public string CategoryName { get; set; }
}