using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager.DataRule.Task
{
	/// <summary>
	/// <see cref="T:IDataManipulationRule"/> that sets <see cref="P:Task.PercentComplete"/> to 100 if <see cref="Task"/> is complete.
	/// </summary>    
    public class SetPercentCompleteTaskDataManipulationRule : TaskWorkflowRuleBase
	{
      
        /// <summary>
        /// Shoulds the process.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            return !target.IsDeleted && target.Status == TaskStatusEnumDto.Completed;
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            if (100 != target.PercentComplete)
            {
                target.PercentComplete = 100;
                context.RecordEntityUpdate(target);
            }

            return true;
        }
	}
}