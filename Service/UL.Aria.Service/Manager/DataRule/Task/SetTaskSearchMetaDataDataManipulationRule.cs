namespace UL.Aria.Service.Manager.DataRule.Task
{
	/// <summary>
	/// <see cref="T:IDataManipulationRule"/> that sets <see cref="Task"/> properties that are needed for search metadata..
	/// </summary>    
    public class SetTaskSearchMetaDataDataManipulationRule : TaskWorkflowRuleBase
	{
        
        /// <summary>
        /// Shoulds the process.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            return !target.IsDeleted;
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            if (target.OrderNumber != context.ActiveProject.OrderNumber ||
                target.CompanyId != context.ActiveProject.CompanyId ||
                target.CompanyName != context.ActiveProject.CompanyName)
            {
                target.OrderNumber = context.ActiveProject.OrderNumber;
                target.CompanyId = context.ActiveProject.CompanyId;
                target.CompanyName = context.ActiveProject.CompanyName;
                context.RecordEntityUpdate(target);
            }
            return true;
        }
	}
}