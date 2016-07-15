using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule.Task
{
	/// <summary>
	/// Class LastTaskCompletedCanceledSetProjectComplete. This class cannot be inherited.
	/// </summary>
	public sealed class LastTaskCompletedCanceledSetProjectComplete : TaskWorkflowRuleBase
	{
		/// <summary>
		/// Determines if this rule should be processed.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool ShouldProcess(IDataRuleContext<Project, Domain.Entity.Task> context, Domain.Entity.Task target)
		{
			var project = context.ActiveProject;
			return !project.IsClosed &&
					project.HasAutoComplete && !project.OverrideAutoComplete &&
			       (target.IsDeleted || target.IsClosed);
		}

		/// <summary>
		/// Processes the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool Process(IDataRuleContext<Project, Domain.Entity.Task> context, Domain.Entity.Task target)
		{

			if (AllProjectTasksAreClosed(context))
			{
				context.ActiveProject.ProjectStatus = ProjectStatusEnumDto.Completed;
				context.IsActiveProjectDirty = true;
				return true;
			}

			return false;
		}

		private static bool AllProjectTasksAreClosed(IDataRuleContext<Project, Domain.Entity.Task> context)
		{
			return
				context.WorkingTasks.Values.Where(task => !task.IsDeleted)
					.All(task => task.IsClosed);
		}
	}
}