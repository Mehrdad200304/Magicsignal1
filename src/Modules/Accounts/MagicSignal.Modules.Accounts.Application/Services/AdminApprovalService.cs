using MagicSignal.Modules.Accounts.Application.Interfaces.Repositories;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Application.DTOs.Workflow;

namespace MagicSignal.Modules.Accounts.Application.Services
{
    public class AdminApprovalService : IAdminApprovalService
    {
        private readonly IAdminApprovalRepository _adminApprovalRepository;

        public AdminApprovalService(IAdminApprovalRepository adminApprovalRepository)
        {
            _adminApprovalRepository = adminApprovalRepository;
        }

        // عملیات اصلی
        public async Task<AdminApproval> ApproveRequestAsync(Guid vipRequestId, Guid adminUserId, string? comment = null)
        {
            // بررسی اینکه قبلاً پردازش نشده باشه
            if (await IsRequestAlreadyProcessedAsync(vipRequestId))
            {
                throw new InvalidOperationException("این درخواست قبلاً پردازش شده است");
            }

            var approval = new AdminApproval
            {
                VipRequestId = vipRequestId,
                AdminUserId = adminUserId,
                IsApproved = true,
                AdminComment = comment ?? "درخواست تأیید شد",
                DecisionDate = DateTime.Now
            };

            return await _adminApprovalRepository.AddAsync(approval);
        }

        public async Task<AdminApproval> RejectRequestAsync(Guid vipRequestId, Guid adminUserId, string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentException("دلیل رد درخواست الزامی است");
            }

            if (await IsRequestAlreadyProcessedAsync(vipRequestId))
            {
                throw new InvalidOperationException("این درخواست قبلاً پردازش شده است");
            }

            var approval = new AdminApproval
            {
                VipRequestId = vipRequestId,
                AdminUserId = adminUserId,
                IsApproved = false,
                AdminComment = comment,
                DecisionDate = DateTime.Now
            };

            return await _adminApprovalRepository.AddAsync(approval);
        }

        // جستجو و دریافت
        public async Task<AdminApproval?> GetApprovalByIdAsync(Guid id)
        {
            return await _adminApprovalRepository.GetByIdAsync(id);
        }

        public async Task<AdminApproval?> GetApprovalByVipRequestAsync(Guid vipRequestId)
        {
            return await _adminApprovalRepository.GetByVipRequestIdAsync(vipRequestId);
        }

        public async Task<IEnumerable<AdminApproval>> GetApprovalsByAdminAsync(Guid adminUserId)
        {
            return await _adminApprovalRepository.GetByAdminUserIdAsync(adminUserId);
        }

        // لیست‌ها
        public async Task<IEnumerable<AdminApproval>> GetPendingApprovalsAsync()
        {
            return await _adminApprovalRepository.GetPendingApprovalsAsync();
        }

        public async Task<IEnumerable<AdminApproval>> GetApprovedRequestsAsync()
        {
            return await _adminApprovalRepository.GetApprovedRequestsAsync();
        }

        public async Task<IEnumerable<AdminApproval>> GetRejectedRequestsAsync()
        {
            return await _adminApprovalRepository.GetRejectedRequestsAsync();
        }

        public async Task<IEnumerable<AdminApproval>> GetRecentDecisionsAsync(int count = 10)
        {
            return await _adminApprovalRepository.GetRecentDecisionsAsync(count);
        }

        // بررسی وضعیت
        public async Task<bool> IsRequestAlreadyProcessedAsync(Guid vipRequestId)
        {
            return await _adminApprovalRepository.IsAlreadyProcessedAsync(vipRequestId);
        }

        public async Task<bool> CanAdminProcessRequestAsync(Guid adminUserId, Guid vipRequestId)
        {
            // اینجا می‌تونیم logic اضافی بذاریم
            // مثلاً چک کنیم ادمین دسترسی داره یا نه
            return !await IsRequestAlreadyProcessedAsync(vipRequestId);
        }

         // آمار
        public async Task<int> GetTotalApprovedCountAsync()
        {
            return await _adminApprovalRepository.GetTotalApprovedCountAsync();
        }

        public async Task<int> GetTotalRejectedCountAsync()
        {
            return await _adminApprovalRepository.GetTotalRejectedCountAsync();
        }

        public async Task<Dictionary<string, int>> GetApprovalStatisticsAsync()
        {
            var approvedCount = await GetTotalApprovedCountAsync();
            var rejectedCount = await GetTotalRejectedCountAsync();
            var totalCount = approvedCount + rejectedCount;

            return new Dictionary<string, int>
            {
                { "Total", totalCount },
                { "Approved", approvedCount },
                { "Rejected", rejectedCount },
                { "ApprovalRate", totalCount > 0 ? (approvedCount * 100 / totalCount) : 0 }
            };
        }

        // DTO conversion
        public async Task<VipStatusDto> GetVipStatusAsync(Guid vipRequestId)
        {
            var approval = await GetApprovalByVipRequestAsync(vipRequestId);
            
            return new VipStatusDto
            {
                UserId = approval?.AdminUserId ?? Guid.Empty,
                IsVip = approval?.IsApproved ?? false,
                ExpirationDate = approval?.IsApproved == true ? DateTime.Now.AddMonths(1) : null,
                PackageType = "Monthly" // فعلاً فیکس
            };
        }

        public async Task<IEnumerable<AdminApprovalDto>> GetApprovalHistoryAsync(Guid vipRequestId)
        {
            var approval = await GetApprovalByVipRequestAsync(vipRequestId);
            if (approval == null) return new List<AdminApprovalDto>();

            return new List<AdminApprovalDto>
            {
                new AdminApprovalDto
                {
                    RequestId = approval.VipRequestId, // حالا Guid
                    IsApproved = approval.IsApproved,
                    AdminComment = approval.AdminComment
                }
            };
        }
    }
}