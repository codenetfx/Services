using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager.DataRule.Task;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Implementation of workflow data context factory.
    /// </summary>
    public class WorkflowDataContextFactory : IWorkflowDataContextFactory
    {
        private readonly IMapperRegistry _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowDataContextFactory"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public WorkflowDataContextFactory(IMapperRegistry mapper) {

            _mapper = mapper;
        }

        /// <summary>
        /// Creates a workflow data context object given the sepecified parameters.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <param name="userAlteredEntities">The user altered entities.</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns></returns>
        public IDataRuleContext<TParent, T> Create<TParent, T>(TParent parent, List<T> userAlteredEntities, bool isDeleteMode = false)
            where TParent : ITrackedDomainEntity
            where T : ITrackedDomainEntity
        {
            return new TaskDataRuleContext(this._mapper, parent as Project, userAlteredEntities as List<Domain.Entity.Task>, isDeleteMode) 
                as IDataRuleContext<TParent, T>;            
        }

    }
}
