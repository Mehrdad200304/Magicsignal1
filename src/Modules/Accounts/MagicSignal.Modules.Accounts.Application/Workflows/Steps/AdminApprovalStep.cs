using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{

    public class AdminApprovalStep : StepBody
    {

        public Guid VipRequestId { get; set; }
        public string? RequestorEmail { get; set; }
        public string? RequestorUsername { get; set; }
        public DateTime RequestDate { get; set; }
// نتیجه تصمیم ادمین        
        public bool IsApproved { get; set; }
        public string? AdminComment { get; set; }
        public Guid AdminUserId { get; set; } 
        public DateTime? DecisionDate { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {

            Console.WriteLine($"AdminApprovalStep: Waiting for admin decision on VIP request {VipRequestId}");

            var eventKey = $"AdminDecision_{VipRequestId}";

            return ExecutionResult.WaitForActivity(eventKey, VipRequestId.ToString(), DateTime.Now.AddHours(24));


        }
    }
}