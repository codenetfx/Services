using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an implemenation for a Business Unit Association Provider.
    /// </summary>
    public class BusinessUnitAssociationProvider:IBusinessUnitAssociationProvider
    {
        private readonly IBusinessUnitAssociationRepository _businessUnitAssociationRepository;
        private readonly IPrincipalResolver _principalResolver;


        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitAssociationProvider" /> class.
        /// </summary>
        /// <param name="businessUnitAssociationRepository">The business unit association repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public BusinessUnitAssociationProvider(IBusinessUnitAssociationRepository businessUnitAssociationRepository, IPrincipalResolver principalResolver)
        {
            _businessUnitAssociationRepository = businessUnitAssociationRepository;
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="businessUnitAssociations">The business units.</param>
        /// <param name="parentId">The parent identifier.</param>
        public void UpdateBulk(IEnumerable<BusinessUnitAssociation> businessUnitAssociations, Guid parentId)
        {
            businessUnitAssociations.ToList().ForEach(x => SetupBusinessUnitAssociation(_principalResolver, x));
            _businessUnitAssociationRepository.UpdateBulk(businessUnitAssociations, parentId);
        }


        /// <summary>
        /// Setups the task.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="businessUnitAssociation">The task.</param>
        internal static void SetupBusinessUnitAssociation(IPrincipalResolver principalResolver, BusinessUnitAssociation businessUnitAssociation)
        {
            var currentDateTime = DateTime.UtcNow;
            businessUnitAssociation.CreatedById = principalResolver.UserId;
            businessUnitAssociation.CreatedDateTime = currentDateTime;
            businessUnitAssociation.UpdatedById = principalResolver.UserId;
            businessUnitAssociation.UpdatedDateTime = currentDateTime;
        }

    }
}
