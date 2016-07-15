using System.Linq;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// Class SetTaskSuccessorsStartDateWhenAllPredecessorsCompletedDataManipulationRule. This class cannot be inherited.
    /// </summary>
    public sealed class SetTaskSuccessorsStartDateWhenAllPredecessorsCompletedDataManipulationRule : TaskWorkflowRuleBase
    {

        /// <summary>
        /// Shoulds the process.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            var originalTask = context.OriginalTasks.ContainsKey(target.Id.GetValueOrDefault()) 
                ? context.OriginalTasks[target.Id.GetValueOrDefault()] : null;
            var originalTaskStatusIsNotClosed = originalTask == null || !originalTask.IsClosed;
            var targetTaskStatusIsCompletedOrCancled = target.IsClosed;

            return originalTaskStatusIsNotClosed && targetTaskStatusIsCompletedOrCancled;
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            target.SuccessorRefs.ForEach(x =>
            {
                var allPredecessorsClosed = x.PredecessorRefs.All(s => s.IsDeleted || s.Status == TaskStatusEnumDto.Completed
                    || s.Status == TaskStatusEnumDto.Canceled);

                if (allPredecessorsClosed)
                {
                    var completedDate = x.PredecessorRefs.Where(y => !y.IsDeleted)
                        .Max(y => y.CompletedDate);

                    // Only change start date when user did not make the start date change.
                    if (!context.OriginalTasks.ContainsKey(x.Id)
                        || x.StartDate == context.OriginalTasks[x.Id].StartDate)
                    {
                        if (x.StartDate != completedDate)
                        {
                            x.StartDate = completedDate;
                            context.RecordEntityUpdate(x);
                        }
                    }
                }
                else
                {
                    

                }

            });


            return true;
        }
    }
}