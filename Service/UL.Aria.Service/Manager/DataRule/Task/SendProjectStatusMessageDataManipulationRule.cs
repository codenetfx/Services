using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
	/// Class SendProjectStatusMessageDataManipulationRule. This class cannot be inherited.
	/// </summary>
    public sealed class SendProjectStatusMessageDataManipulationRule : TaskWorkflowRuleBase
	{
		private readonly IProjectStatusMessageProvider _projectStatusMessageProvider;
        private const string TriggerKey = "ShouldTriggerBillingProcessed";
        private Func<Domain.Entity.Task, bool> ShouldTriggerBillingFunc
        {
            get 
            {
                return (Domain.Entity.Task task) =>
                 {
                     return !task.IsDeleted && task.ShouldTriggerBilling && task.Status == TaskStatusEnumDto.Completed;
                 };
            }
                
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SendProjectStatusMessageDataManipulationRule" /> class.
		/// </summary>
		/// <param name="projectStatusMessageProvider">The project status message provider.</param>
		public SendProjectStatusMessageDataManipulationRule(IProjectStatusMessageProvider projectStatusMessageProvider)
		{
			_projectStatusMessageProvider = projectStatusMessageProvider;
		}

        /// <summary>
        /// Shoulds the process.
        /// </summary>
        /// <param name="ruleContext">The rule context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> ruleContext, Domain.Entity.Task target)
        {
			//Don't delete we are gonig to enable it later.
			//var shouldTriggerBillingProcessedHasRan = (ruleContext.WorkflowState.ContainsKey(TriggerKey)
			//	&& (ruleContext.WorkflowState[TriggerKey] as bool?).GetValueOrDefault());
			//var isExternallyCreatedProject = !string.IsNullOrWhiteSpace(ruleContext.ActiveProject.ExternalProjectId);
			//var projectHasCompletedTasksWithBillingTriggers = ruleContext.ActiveProject.Tasks.Any(ShouldTriggerBillingFunc);

			//return !shouldTriggerBillingProcessedHasRan &&
			//		isExternallyCreatedProject &&
			//		projectHasCompletedTasksWithBillingTriggers;

	        return false;
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="ruleContext">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> ruleContext, Domain.Entity.Task target)
        {
			//Don't delete. We are going enable at later point.

			//var project = ruleContext.ActiveProject;
			//bool triggerBilling = false;

			//foreach (var task in project.Tasks.Where(task => task.ShouldTriggerBilling))
			//{
			//	if (task.IsDeleted)
			//		continue;

			//	switch (task.Status)
			//	{
			//		case TaskStatusEnumDto.Completed:
			//			triggerBilling = true;
			//			continue;
			//		case TaskStatusEnumDto.Canceled:
			//			continue;
			//	}
			//	triggerBilling = false;
			//	break;
			//}

			//if (triggerBilling)
			//{
			//	var changedLines = new List<ProjectStatusMessageServiceLine>();
			//	changedLines.AddRange(
			//		project.ServiceLines.Select(
			//			x =>
			//				new ProjectStatusMessageServiceLine
			//				{
			//					ProjectServiceLineId = x.Id.GetValueOrDefault(),
			//					ServiceLineAction = ServiceLineAction.NoChange,
			//					ServiceLine = x
			//				}));
			//	_projectStatusMessageProvider.Publish(new ProjectStatusMessage
			//	{
			//		// ReSharper disable once PossibleInvalidOperationException
			//		ProjectId = project.Id.Value,
			//		OldStatus = project.ProjectStatus,
			//		NewStatus = ProjectStatusEnumDto.Completed,
			//		EventId = BusinessMessageIds.Projects.ProjectStatusMessage,
			//		ExternalProjectId = project.ExternalProjectId,
			//		ProjectName = project.ProjectName,
			//		ProjectNumber = project.ProjectNumber,
			//		OrderNumber = project.OrderNumber,
			//		ProjectServiceLineStatuses = changedLines,
			//		WorkOrderBusinessComponentId = project.WorkOrderBusinessComponentId,
			//		WorkOrderId = project.WorkOrderId
			//	});
			//}

			//ruleContext.WorkflowState[TriggerKey] = true;

            return true;
        }

       
	}
}