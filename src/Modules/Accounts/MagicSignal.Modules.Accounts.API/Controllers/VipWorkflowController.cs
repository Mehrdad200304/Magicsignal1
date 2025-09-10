using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using MagicSignal.Modules.Accounts.Application.DTOs.Workflow;
using MagicSignal.Modules.Accounts.Application.Workflows;
using MagicSignal.Modules.Accounts.Application.Workflows.Steps;
using Microsoft.AspNetCore.Authorization;

namespace MagicSignal.Modules.Accounts.API.Controllers
{
    [ApiController]
    [Route("api/vip-workflow")]
    //[Authorize]
    public class VipWorkflowController : ControllerBase
    {
        private readonly IWorkflowHost _workflowHost;

        public VipWorkflowController(IWorkflowHost workflowHost)
        {
            _workflowHost = workflowHost;
        }

        [HttpPost("register")]
        public async Task<IActionResult> StartVipRegistration([FromBody] VipRegistrationRequestDto request)
        {
            try
            {
                // Validation
                if (request == null)
                {
                    return BadRequest(new VipRegistrationResponseDto
                    {
                        Success = false,
                        Message = "داده‌های درخواست نامعتبر است"
                    });
                }

                if (request.Amount <= 0)
                {
                    return BadRequest(new VipRegistrationResponseDto
                    {
                        Success = false,
                        Message = "مبلغ پرداختی باید بیشتر از صفر باشد"
                    });
                }

                // ساخت داده‌های workflow
                var workflowData = new MagicSignal.Modules.Accounts.Application.Workflows.VipWorkflowData
                {
                    UserId = new Guid($"00000000-0000-0000-0000-{request.UserId:D12}"),
                    PackageType = request.PackageType,
                    Amount = request.Amount,
                    PaymentMethod = request.PaymentMethod,
                    CreatedAt = DateTime.Now,
                    CurrentStep = "Starting",
                    VipRequestId = Guid.NewGuid(), // یک ID جدید
                    Username = "TempUser", // موقتی
                    UserEmail = "temp@example.com", // موقتی
                    RequestDate = DateTime.Now,
                    IsApproved = false,
                    AdminUserId = Guid.Empty,
                    PaymentStatus = request.PaymentMethod ?? "Unknown", // از PaymentMethod
                    PaymentCompleted = false,
                    AdminApproved = false
                };

                // شروع workflow
                var workflowId = await _workflowHost.StartWorkflow("VipApprovalWorkflow", workflowData);

                return Ok(new VipRegistrationResponseDto
                {
                    Success = true,
                    Message = "درخواست VIP شما با موفقیت ثبت شد",
                    RequestId = int.Parse(workflowId.Substring(0, 8), System.Globalization.NumberStyles.HexNumber),
                    TransactionId = workflowId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new VipRegistrationResponseDto
                {
                    Success = false,
                    Message = "خطا در ثبت درخواست VIP: " + ex.Message
                });
            }
        }

        [HttpGet("status/{requestId}")]
        public async Task<IActionResult> GetVipRegistrationStatus(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "شناسه درخواست نامعتبر است"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    RequestId = requestId,
                    Status = "InProgress",
                    CurrentStep = "WaitingForAdminApproval", 
                    Message = "درخواست شما در حال بررسی توسط ادمین است",
                    CreatedAt = DateTime.Now.AddHours(-1)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطا در دریافت وضعیت درخواست",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("admin-approval")]
        public async Task<IActionResult> AdminApproval([FromBody] AdminApprovalDto approval)
        {
            try
            {
                if (approval == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "داده‌های تأیید نامعتبر است"
                    });
                }

                if (approval.RequestId == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "شناسه درخواست نامعتبر است"
                    });
                }

                var message = approval.IsApproved 
                    ? "درخواست VIP تأیید شد و فعال‌سازی انجام خواهد شد"
                    : "درخواست VIP رد شد";

                return Ok(new
                {
                    Success = true,
                    RequestId = approval.RequestId,
                    IsApproved = approval.IsApproved,
                    AdminComment = approval.AdminComment,
                    Message = message,
                    ProcessedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطا در پردازش تأیید ادمین",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("cancel/{requestId}")]
        public async Task<IActionResult> CancelVipRequest(int requestId)
        {
            try
            {
                return Ok(new
                {
                    RequestId = requestId,
                    Success = true,
                    Message = "درخواست VIP شما لغو شد",
                    CancelledAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "خطا در لغو درخواست",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("admin/requests")]
        public async Task<IActionResult> GetAllVipRequests()
        {
            try
            {
                var requests = new[]
                {
                    new { RequestId = 1, UserId = 100, PackageType = "Monthly", Amount = 50, Status = "Pending" },
                    new { RequestId = 2, UserId = 101, PackageType = "Yearly", Amount = 500, Status = "InProgress" },
                    new { RequestId = 3, UserId = 102, PackageType = "Monthly", Amount = 50, Status = "Approved" }
                };

                return Ok(new
                {
                    Success = true,
                    Data = requests,
                    Count = requests.Length
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "خطا در دریافت لیست درخواست‌ها",
                    Error = ex.Message
                });
            }
        }
    }
}