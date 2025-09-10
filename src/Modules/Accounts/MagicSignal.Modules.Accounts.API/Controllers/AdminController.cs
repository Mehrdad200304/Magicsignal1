using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using MagicSignal.Modules.Accounts.Application.DTOs.Workflow;
using Microsoft.AspNetCore.Authorization;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;  
namespace MagicSignal.Modules.Accounts.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]  // فقط Admin ها دسترسی دارن
    public class AdminController : ControllerBase
    {
        private readonly IWorkflowHost _workflowHost;
        private readonly IAdminApprovalService _adminApprovalService;
        public AdminController(
            IWorkflowHost workflowHost,
            IAdminApprovalService adminApprovalService)
        {
            _workflowHost = workflowHost;
            _adminApprovalService = adminApprovalService;
        }

        [HttpGet("pending-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                // استفاده از Service واقعی بجای Mock Data
                var pendingRequests = await _adminApprovalService.GetPendingApprovalsAsync();
        
                return Ok(new
                {
                    Success = true,
                    Message = "درخواست‌های در انتظار تأیید",
                    Data = pendingRequests.Select(req => new
                    {
                        RequestId = req.Id,
                        VipRequestId = req.VipRequestId,
                        AdminUserId = req.AdminUserId,
                        IsApproved = req.IsApproved,
                        AdminComment = req.AdminComment,
                        CreatedAt = req.CreatedAt
                    }),
                    Count = pendingRequests.Count()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطا در دریافت درخواست‌ها",
                    Error = ex.Message
                });
            }
        }
        
        [HttpPost("approve/{vipRequestId}")]
        public async Task<IActionResult> ApproveRequest(Guid vipRequestId, [FromBody] string? adminComment = null)
        {
            try
            {
                if (vipRequestId == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "شناسه درخواست نامعتبر است"
                    });
                }

                // فعلاً AdminUserId رو فیکس می‌کنیم - بعداً از JWT می‌گیریم
                var adminUserId = Guid.NewGuid(); // موقتی
        
                // استفاده از Service واقعی
                var approval = await _adminApprovalService.ApproveRequestAsync(
                    vipRequestId, 
                    adminUserId, 
                    adminComment ?? "درخواست تأیید شد"
                );

                // 🚀 تریگر کردن Workflow Event
                var eventKey = $"AdminDecision_{vipRequestId}";
                var eventData = new 
                {
                    VipRequestId = vipRequestId,
                    IsApproved = true,
                    AdminComment = adminComment ?? "درخواست تأیید شد",
                    AdminUserId = adminUserId,
                    DecisionDate = DateTime.Now
                };

                // ارسال Event به Workflow
                await _workflowHost.PublishEvent(eventKey, vipRequestId.ToString(), eventData);

                Console.WriteLine($"Workflow Event Published: {eventKey} for VIP Request {vipRequestId}");

                return Ok(new
                {
                    Success = true,
                    RequestId = approval.Id,
                    VipRequestId = approval.VipRequestId,
                    Action = "Approved",
                    AdminComment = approval.AdminComment,
                    Message = "درخواست VIP تأیید شد",
                    ProcessedAt = approval.DecisionDate,
                    WorkflowTriggered = true
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطا در تأیید درخواست",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("reject/{vipRequestId}")]
        public async Task<IActionResult> RejectRequest(Guid vipRequestId, [FromBody] string adminComment)
        {
            try
            {
                if (vipRequestId == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "شناسه درخواست نامعتبر است"
                    });
                }

                if (string.IsNullOrEmpty(adminComment))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "دلیل رد درخواست الزامی است"
                    });
                }

                // فعلاً AdminUserId رو فیکس می‌کنیم - بعداً از JWT می‌گیریم
                var adminUserId = Guid.NewGuid(); // موقتی
        
                // استفاده از Service واقعی
                var approval = await _adminApprovalService.RejectRequestAsync(
                    vipRequestId, 
                    adminUserId, 
                    adminComment
                );

                // 🚀 تریگر کردن Workflow Event
                var eventKey = $"AdminDecision_{vipRequestId}";
                
                var eventData = new 
                {
                    VipRequestId = vipRequestId,
                    IsApproved = false,
                    AdminComment = adminComment,
                    AdminUserId = adminUserId,
                    DecisionDate = DateTime.Now
                };

                // ارسال Event به Workflow
                await _workflowHost.PublishEvent(eventKey, vipRequestId.ToString(), eventData);

                Console.WriteLine($"Workflow Event Published: {eventKey} for VIP Request {vipRequestId} - REJECTED");

                return Ok(new
                {
                    Success = true,
                    RequestId = approval.Id,
                    VipRequestId = approval.VipRequestId,
                    Action = "Rejected",
                    AdminComment = approval.AdminComment,
                    Message = "درخواست VIP رد شد",
                    ProcessedAt = approval.DecisionDate,
                    WorkflowTriggered = true
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطا در رد درخواست",
                    Error = ex.Message
                });
            }
        }
        
        [HttpGet("admin-users")]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isVip = null)
        {
            try
            {
                // Validation
                if (page <= 0) page = 1;
                if (pageSize <= 0 || pageSize > 100) pageSize = 10;

                // Mock Data - بعداً با UserService جایگزین می‌شه
                var allUsers = new[]
                {
                    new
                    {
                        UserId = 100, Username = "user1", Email = "user1@example.com", IsVip = false,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    new
                    {
                        UserId = 101, Username = "user2", Email = "user2@example.com", IsVip = true,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new
                    {
                        UserId = 102, Username = "user3", Email = "user3@example.com", IsVip = false,
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    new
                    {
                        UserId = 103, Username = "vipUser1", Email = "vip1@example.com", IsVip = true,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    new
                    {
                        UserId = 104, Username = "normalUser", Email = "normal@example.com", IsVip = false,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    }
                };

                // Filter by VIP status if specified
                var filteredUsers = isVip.HasValue
                    ? allUsers.Where(u => u.IsVip == isVip.Value)
                    : allUsers;

                // Pagination
                var pagedUsers = filteredUsers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var totalCount = filteredUsers.Count();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return Ok(new
                {
                    Success = true,
                    Message = "لیست کاربران",
                    Data = pagedUsers,
                    Count = pagedUsers.Count,
                    Pagination = new
                    {
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages,
                        HasNextPage = page < totalPages,
                        HasPreviousPage = page > 1
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطا در دریافت لیست کاربران",
                    Error = ex.Message
                });
            }
        }
    }
}