using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// Deletes any decendants of target task that are not already marked for deletion.
    /// </summary>
    public class DeletedTaskDeletesAllChildren : TaskWorkflowRuleBase
    {
        /// <summary>
        /// Determines if this rule should be processed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool ShouldProcess(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            return (target.IsDeleted && target.SubTasks.Any(x=> !x.IsDeleted));
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override bool Process(IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task> context, Domain.Entity.Task target)
        {
            var result = false;
            target.SubTasks.Where(x => !x.IsDeleted).ToList()
                .ForEach( x=> {
                context.RecordEntityDelete(x);
                result = true;
            });

            return result;
        }
    }
}
