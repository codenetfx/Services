using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// Task Delete Rule that when a task is being deleted, its Successor/Predecessor relationships are removed.
    /// /// </summary>
    public class DeletedTaskRemoveSuccessorPredecessorRelationships : TaskWorkflowRuleBase
    {
        /// <summary>
        /// Determines if this rule should be processed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            return target.IsDeleted && (target.PredecessorRefs.Any() || target.SuccessorRefs.Any() || target.Predecessors.Any());
        }

        /// <summary>
        /// Processes the rule given specified context and target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            var result = false;
            target.SuccessorRefs.ForEach(targetSuccessor =>
            {
                targetSuccessor.Predecessors.RemoveAll(x => x.TaskId == target.Id);
                targetSuccessor.PredecessorRefs.Remove(target);
                context.RecordEntityUpdate(targetSuccessor);
                result = true;
            });

            target.PredecessorRefs.ForEach(targetPredecessor =>
            {
                targetPredecessor.SuccessorRefs.Remove(target);
                result = true;
            });

            if (target.Predecessors.Any())
            {
                target.Predecessors.Clear();
                context.RecordEntityUpdate(target);
                result = true;               
            }

            return result;
        }
    }
}
