using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// The Workflow rule interface
    /// </summary>
    public interface ITaskWorkflowRule : IDataManipulationRule<IDataRuleContext<Project, Domain.Entity.Task>, Domain.Entity.Task> { }

    /// <summary>
    /// The Task workflow rule base.
    /// </summary>
    public abstract class TaskWorkflowRuleBase : ITaskWorkflowRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWorkflowRuleBase"/> class.
        /// </summary>
        protected TaskWorkflowRuleBase() { }

        /// <summary>
        /// Determines if this rule should be processed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public abstract bool ShouldProcess(IDataRuleContext<Project, Domain.Entity.Task> context,
            Domain.Entity.Task target);

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public abstract bool Process(IDataRuleContext<Project, Domain.Entity.Task> context, Domain.Entity.Task target);
    }
}