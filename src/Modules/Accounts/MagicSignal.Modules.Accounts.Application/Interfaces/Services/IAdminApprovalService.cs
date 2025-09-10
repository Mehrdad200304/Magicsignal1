using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Application.DTOs.Workflow;


namespace MagicSignal.Modules.Accounts.Application.Interfaces.Services
{
    public interface IAdminApprovalService
    {
        // عملیات اصلی
        Task<AdminApproval> ApproveRequestAsync(Guid vipRequestId, Guid adminUserId, string? comment = null);
        Task<AdminApproval> RejectRequestAsync(Guid vipRequestId, Guid adminUserId, string comment);
        
        // جستجو و دریافت
        Task<AdminApproval?> GetApprovalByIdAsync(Guid id);
        Task<AdminApproval?> GetApprovalByVipRequestAsync(Guid vipRequestId);
        Task<IEnumerable<AdminApproval>> GetApprovalsByAdminAsync(Guid adminUserId);
        
        // لیست‌ها
        Task<IEnumerable<AdminApproval>> GetPendingApprovalsAsync();
        Task<IEnumerable<AdminApproval>> GetApprovedRequestsAsync();
        Task<IEnumerable<AdminApproval>> GetRejectedRequestsAsync();
        Task<IEnumerable<AdminApproval>> GetRecentDecisionsAsync(int count = 10);
        
        // بررسی وضعیت
        Task<bool> IsRequestAlreadyProcessedAsync(Guid vipRequestId);
        Task<bool> CanAdminProcessRequestAsync(Guid adminUserId, Guid vipRequestId);
        
        // آمار
        Task<int> GetTotalApprovedCountAsync();
        Task<int> GetTotalRejectedCountAsync();
        Task<Dictionary<string, int>> GetApprovalStatisticsAsync();
        
        // DTO conversion
        Task<VipStatusDto> GetVipStatusAsync(Guid vipRequestId);
        Task<IEnumerable<AdminApprovalDto>> GetApprovalHistoryAsync(Guid vipRequestId);
     
    }
}