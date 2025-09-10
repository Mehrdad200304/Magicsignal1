using MagicSignal.Modules.Accounts.Domain.Entities;

namespace MagicSignal.Modules.Accounts.Application.Interfaces.Repositories
{
    public interface IAdminApprovalRepository
    {
        // CRUD عملیات پایه
        Task<AdminApproval?> GetByIdAsync(Guid id);
        Task<IEnumerable<AdminApproval>> GetAllAsync();
        Task<AdminApproval> AddAsync(AdminApproval adminApproval);
        Task UpdateAsync(AdminApproval adminApproval);
        Task DeleteAsync(Guid id);
        
        // عملیات تخصصی
        Task<AdminApproval?> GetByVipRequestIdAsync(Guid vipRequestId);
        Task<IEnumerable<AdminApproval>> GetByAdminUserIdAsync(Guid adminUserId);
        Task<IEnumerable<AdminApproval>> GetPendingApprovalsAsync();
        Task<IEnumerable<AdminApproval>> GetApprovedRequestsAsync();
        Task<IEnumerable<AdminApproval>> GetRejectedRequestsAsync();
        
        // بررسی وجود
        Task<bool> ExistsAsync(Guid id);
        Task<bool> IsAlreadyProcessedAsync(Guid vipRequestId);
        
        // آمار و گزارش
        Task<int> GetTotalApprovedCountAsync();
        Task<int> GetTotalRejectedCountAsync();
        Task<IEnumerable<AdminApproval>> GetRecentDecisionsAsync(int count = 10);
    }
}