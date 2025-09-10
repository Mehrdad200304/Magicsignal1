using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        // خواندن
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);

        // نوشتن
        Task<Category> AddAsync(Category entity);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(Guid id);

        // عملیات خاص
        Task<bool> ExistsAsync(Guid id);
    }
}