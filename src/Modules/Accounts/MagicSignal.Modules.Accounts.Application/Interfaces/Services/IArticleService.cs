using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Services
{
    public interface IArticleService
    {
        // خواندن
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(Guid id);
        Task<IEnumerable<Article>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Article>> GetByAuthorIdAsync(Guid authorId);
        Task<IEnumerable<Article>> GetPublishedArticlesAsync();
        
        // نوشتن
        Task<Article> AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(Guid id);
        
        // عملیات خاص
        Task<bool> ExistsAsync(Guid id);
        Task IncrementViewCountAsync(Guid id);
    }
}