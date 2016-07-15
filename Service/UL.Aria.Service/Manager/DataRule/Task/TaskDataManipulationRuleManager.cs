using System;
using System.Linq;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// Implements operations to manage data dataManipulationRules for tasks.
    /// </summary>
    public class TaskDataManipulationRuleManager: WorkflowManager<Project, Domain.Entity.Task>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDataManipulationRuleManager"/> class.
        /// </summary>
        /// <param name="workflowRules">The workflow rules.</param>
        public TaskDataManipulationRuleManager(IEnumerable<ITaskWorkflowRule> workflowRules) 
            : base(workflowRules.Cast<IDataManipulationRule<IDataRuleContext<Project, Domain.Entity.Task>, Domain.Entity.Task>>()) { }
    }
}
