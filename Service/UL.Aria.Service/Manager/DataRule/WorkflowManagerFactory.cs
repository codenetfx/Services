using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;


namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowManagerFactory : IWorkflowManagerFactory
    {
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowManagerFactory"/> class.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        public WorkflowManagerFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// Creates the specified workflow name.
        /// </summary>
        /// <param name="workflowName">Name of the workflow.</param>
        /// <returns></returns>
        public IDataRuleManager<IDataRuleContext<TParent, T>> Create<TParent, T>(Workflow workflowName = Workflow.Default)
            where TParent : ITrackedDomainEntity
            where T : ITrackedDomainEntity
        {
            if (workflowName != Workflow.Default)
            {
                return _unityContainer.Resolve<IDataRuleManager<IDataRuleContext<TParent, T>>>(workflowName.ToString());
            }
            return _unityContainer.Resolve<IDataRuleManager<IDataRuleContext<TParent, T>>>();
        }
    }
}