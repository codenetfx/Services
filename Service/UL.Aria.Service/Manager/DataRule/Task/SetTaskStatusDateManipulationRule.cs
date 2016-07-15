using UL.Aria.Service.TaskStatus;

namespace UL.Aria.Service.Manager.DataRule.Task
{
	/// <summary>
	/// Class SetTaskStatusDateManipulationRule. This class cannot be inherited.
	/// </summary>
    public sealed class SetTaskStatusDateManipulationRule : TaskWorkflowRuleBase
	{
		private readonly ITaskFetchStatusStrategyFactory _taskFetchStatusStrategyFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetTaskStatusDateManipulationRule"/> class.
		/// </summary>
		/// <param name="taskFetchStatusStrategyFactory">The task fetch status strategy factory.</param>
		public SetTaskStatusDateManipulationRule(ITaskFetchStatusStrategyFactory taskFetchStatusStrategyFactory)
		{
			_taskFetchStatusStrategyFactory = taskFetchStatusStrategyFactory;
		}

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
            var status = target.Status;
            target.Status = _taskFetchStatusStrategyFactory.GetStrategy(target.Status).FetchTaskStatus(target);

            if (status != target.Status)
            {
                context.RecordEntityUpdate(target);
                return true;
            }

            return false;
        }
	}
}