using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Workflow Manager Factory Interface
    /// </summary>
    public interface IWorkflowManagerFactory
    {
        /// <summary>
        /// Creates the specified workflow name.
        /// </summary>
        /// <param name="workflowName">Name of the workflow.</param>
        /// <returns></returns>
        IDataRuleManager<IDataRuleContext<TParent, T>> Create<TParent, T>(Workflow workflowName = Workflow.Default)
            where TParent : ITrackedDomainEntity
            where T : ITrackedDomainEntity;
         
    }
}