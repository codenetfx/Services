namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// <see cref="T:IDataManipulationRule"/> that sets <see cref="P:Task.TaskOwner"/> to all lower case.
    /// </summary>    
    public class TaskOwnerToLowerTaskDataManipulationRule : TaskWorkflowRuleBase
    {

        /// <summary>
        /// Determines if this rule should be processed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            return !target.IsDeleted && target.HasTaskOwner;
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            if (target.TaskOwner.ToLower() != target.TaskOwner)
            {
                target.TaskOwner = target.TaskOwner.ToLower();
                context.RecordEntityUpdate(target);
            }

            return true;
        }
    }
}