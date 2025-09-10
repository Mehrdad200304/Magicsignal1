using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        // متدهای خاص برای Article
        Task<IEnumerable<Article>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Article>> GetByAuthorIdAsync(Guid authorId);
        Task<IEnumerable<Article>> GetPublishedArticlesAsync();
        Task<bool> ExistsAsync(Guid id);
        Task IncrementViewCountAsync(Guid id);
    }
}