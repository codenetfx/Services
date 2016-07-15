using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Implements operations to manage data dataManipulationRules for tasks.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract class WorkflowManager<TParent, T> : IDataRuleManager<IDataRuleContext<TParent, T>>
        where TParent : ITrackedDomainEntity
        where T : ITrackedDomainEntity
    {
        private readonly IEnumerable<IDataManipulationRule<IDataRuleContext<TParent, T>, T>> _dataManipulationRules;


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowManager{TParent, T}"/> class.
        /// </summary>
        /// <param name="dataManipulationRules">The data manipulation rules.</param>
        protected WorkflowManager(IEnumerable<IDataManipulationRule<IDataRuleContext<TParent, T>, T>> dataManipulationRules)
        {
            _dataManipulationRules = dataManipulationRules;
        }

        /// <summary>
        /// Process the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool Process(IDataRuleContext<TParent, T> context)
        {
            const int maxRuleIterations = 1000;
            int ruleIteration = 0;

            while (context.ProcessingQueue.Count > 0)
            {
                var target = context.ProcessingQueue.Dequeue();
                foreach (var dataManipulationRule in _dataManipulationRules)
                {
                   
                    if (dataManipulationRule.ShouldProcess(context, target))
                    {
                        dataManipulationRule.Process(context, target);
                    }                  
                    
                }
                if (++ruleIteration > maxRuleIterations)
                {
                    throw new InvalidOperationException("Max number of rule iterations exceeded");
                }

            }
            return context.HasDirtyEntities();
        }
    }
}