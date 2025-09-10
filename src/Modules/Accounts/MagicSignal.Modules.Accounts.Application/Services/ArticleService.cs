using MagicSignal.Modules.Accounts.Application.Interfaces.Repositories;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _articleRepository.GetAllAsync();
        }

        public async Task<Article?> GetByIdAsync(Guid id)
        {
            return await _articleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Article>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _articleRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<Article>> GetByAuthorIdAsync(Guid authorId)
        {
            return await _articleRepository.GetByAuthorIdAsync(authorId);
        }

        public async Task<IEnumerable<Article>> GetPublishedArticlesAsync()
        {
            return await _articleRepository.GetPublishedArticlesAsync();
        }

        public async Task<Article> AddAsync(Article article)
        {
            return await _articleRepository.AddAsync(article);
        }

        public async Task UpdateAsync(Article article)
        {
            await _articleRepository.UpdateAsync(article);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _articleRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _articleRepository.ExistsAsync(id);
        }

        public async Task IncrementViewCountAsync(Guid id)
        {
            await _articleRepository.IncrementViewCountAsync(id);
        }
    }
}