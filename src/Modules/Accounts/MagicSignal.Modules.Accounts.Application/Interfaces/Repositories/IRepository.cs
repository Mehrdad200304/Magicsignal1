using System.Linq.Expressions;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);                // تغییر: Task<T> به جای Task
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);                 // تغییر: Guid id به جای T entity
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Guid id);          // اضافه کنید
    }
}