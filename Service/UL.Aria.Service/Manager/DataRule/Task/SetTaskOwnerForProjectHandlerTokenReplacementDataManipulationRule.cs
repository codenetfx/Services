using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager.DataRule.Task
{
	/// <summary>
	/// Applies token replacement to the TaskOwner Member when the ProjectHandler Token is found
	/// </summary>
    public class SetTaskOwnerForProjectHandlerTokenReplacementDataManipulationRule : TaskWorkflowRuleBase
	{
        /// <summary>
        /// Shoulds the process.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            return !string.IsNullOrWhiteSpace(target.TaskOwner)
                   && !target.TaskOwner.Contains("@")
                   && target.TaskOwner == AssetFieldNames.ProjectHandlerToken;
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            var isProjectHandlerTaskOwner = string.Equals(context.ActiveProject.ProjectHandler, target.TaskOwner,
                System.StringComparison.OrdinalIgnoreCase);
            if (!isProjectHandlerTaskOwner)
            {
                target.TaskOwner = context.ActiveProject.ProjectHandler;
                context.RecordEntityUpdate(target);
            }

            return true;
        }

    }
}