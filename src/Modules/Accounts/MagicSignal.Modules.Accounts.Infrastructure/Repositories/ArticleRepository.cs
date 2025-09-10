using MagicSignal.Modules.Accounts.Application.Interfaces.Repositories;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MagicSignal.Modules.Accounts.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Article?> GetByIdAsync(Guid id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles
                .Include(a => a.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Articles
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetByAuthorIdAsync(Guid authorId)
        {
            return await _context.Articles
                .Where(a => a.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetPublishedArticlesAsync()
        {
            return await _context.Articles
                .Where(a => a.IsPublished)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Articles.AnyAsync(a => a.Id == id);
        }

        public async Task IncrementViewCountAsync(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Article> AddAsync(Article entity)
        {
            await _context.Articles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity; // اضافه شد
        }

        public async Task UpdateAsync(Article entity)
        {
            _context.Articles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Article>> FindAsync(System.Linq.Expressions.Expression<Func<Article, bool>> predicate)
        {
            return await _context.Articles.Where(predicate).ToListAsync();
        }
    }
}