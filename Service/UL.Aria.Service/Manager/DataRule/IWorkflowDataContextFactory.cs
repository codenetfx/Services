using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Workflow Data context factory
    /// </summary>
    public interface IWorkflowDataContextFactory
    {

        /// <summary>
        /// Creates a workflow data context object given the sepecified parameters.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <param name="userAlteredEntities">The user altered entities.</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns></returns>
        IDataRuleContext<TParent, T> Create<TParent, T>(TParent parent, List<T> userAlteredEntities, bool isDeleteMode = false)
            where TParent: ITrackedDomainEntity
            where T: ITrackedDomainEntity;
    }
}
