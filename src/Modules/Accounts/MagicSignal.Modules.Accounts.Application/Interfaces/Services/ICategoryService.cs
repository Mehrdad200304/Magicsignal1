using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        // خواندن
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        
        // نوشتن
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
        
        // عملیات خاص
        Task<bool> ExistsAsync(Guid id);
    }
}