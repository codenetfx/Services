using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Defines data manipulation rules for Tasks.
    /// </summary>
    public interface IDataManipulationRule<in T, in TE> where T: IDataRuleContext where TE: ITrackedDomainEntity
    {
        
        /// <summary>
        /// Determines if this rule should be processed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        bool ShouldProcess(T context, TE target);

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        bool Process(T context, TE target);
    }
}
