using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace MagicSignal.Modules.Accounts.Application.Workflows.Steps
{
    public abstract class BaseWorkflowStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            try
            {
                return ExecuteStep(context);
            }
            catch (Exception ex)
            {
                // Log error here
                return ExecutionResult.Outcome($"Error: {ex.Message}");
            }
        }

        protected abstract ExecutionResult ExecuteStep(IStepExecutionContext context);
    }
}