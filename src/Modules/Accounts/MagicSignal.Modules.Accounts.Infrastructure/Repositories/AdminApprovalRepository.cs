using Microsoft.EntityFrameworkCore;
using MagicSignal.Modules.Accounts.Application.Interfaces.Repositories;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence;


namespace MagicSignal.Modules.Accounts.Infrastructure.Repositories
{
    public class AdminApprovalRepository : IAdminApprovalRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminApprovalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CRUD عملیات پایه
        public async Task<AdminApproval?> GetByIdAsync(Guid id)
        {
            return await _context.AdminApprovals
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<IEnumerable<AdminApproval>> GetAllAsync()
        {
            return await _context.AdminApprovals
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<AdminApproval> AddAsync(AdminApproval adminApproval)
        {
            adminApproval.Id = Guid.NewGuid();
            adminApproval.CreatedAt = DateTime.Now;
            adminApproval.IsActive = true;

            _context.AdminApprovals.Add(adminApproval);
            await _context.SaveChangesAsync();
            return adminApproval;
        }

        public async Task UpdateAsync(AdminApproval adminApproval)
        {
            adminApproval.UpdatedAt = DateTime.Now;
            _context.AdminApprovals.Update(adminApproval);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var adminApproval = await GetByIdAsync(id);
            if (adminApproval != null)
            {
                adminApproval.IsActive = false;
                adminApproval.UpdatedAt = DateTime.Now;
                await UpdateAsync(adminApproval);
            }
        }

        // عملیات تخصصی
        public async Task<AdminApproval?> GetByVipRequestIdAsync(Guid vipRequestId)
        {
            return await _context.AdminApprovals
                .FirstOrDefaultAsync(x => x.VipRequestId == vipRequestId && x.IsActive);
        }

        public async Task<IEnumerable<AdminApproval>> GetByAdminUserIdAsync(Guid adminUserId)
        {
            return await _context.AdminApprovals
                .Where(x => x.AdminUserId == adminUserId && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminApproval>> GetPendingApprovalsAsync()
        {
            // در واقعیت باید از VipRequest entity چک کنیم
            // فعلاً همه رکوردهایی که امروز ساخته شدن رو برمی‌گردونیم
            var today = DateTime.Today;
            return await _context.AdminApprovals
                .Where(x => x.IsActive && x.CreatedAt >= today)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminApproval>> GetApprovedRequestsAsync()
        {
            return await _context.AdminApprovals
                .Where(x => x.IsActive && x.IsApproved == true)
                .OrderByDescending(x => x.DecisionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminApproval>> GetRejectedRequestsAsync()
        {
            return await _context.AdminApprovals
                .Where(x => x.IsActive && x.IsApproved == false)
                .OrderByDescending(x => x.DecisionDate)
                .ToListAsync();
        }

        // بررسی وجود
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.AdminApprovals
                .AnyAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<bool> IsAlreadyProcessedAsync(Guid vipRequestId)
        {
            return await _context.AdminApprovals
                .AnyAsync(x => x.VipRequestId == vipRequestId && x.IsActive);
        }
        // آمار و گزارش
        public async Task<int> GetTotalApprovedCountAsync()
        {
            return await _context.AdminApprovals
                .CountAsync(x => x.IsActive && x.IsApproved == true);
        }

        public async Task<int> GetTotalRejectedCountAsync()
        {
            return await _context.AdminApprovals
                .CountAsync(x => x.IsActive && x.IsApproved == false);
        }

        public async Task<IEnumerable<AdminApproval>> GetRecentDecisionsAsync(int count = 10)
        {
            return await _context.AdminApprovals
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.DecisionDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
