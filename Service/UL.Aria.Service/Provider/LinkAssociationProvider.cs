using System;
using System.Linq;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides a Provider implemenatiion for Link Associations.
    /// </summary>
	public class LinkAssociationProvider : ILinkAssociationProvider
	{
		private readonly ILinkAssociationRepository _linkAssociationRepository;
	    private readonly IPrincipalResolver _principalResolver;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="LinkAssociationProvider"/> class.
	    /// </summary>
	    /// <param name="linkAssociationRepository">The link association repository.</param>
	    /// <param name="principalResolver"></param>
	    public LinkAssociationProvider(ILinkAssociationRepository linkAssociationRepository, IPrincipalResolver principalResolver)
	    {
		    _linkAssociationRepository = linkAssociationRepository;
		    _principalResolver = principalResolver;
	    }

	    /// <summary>
        /// Updates the link associations.
        /// </summary>
        /// <param name="linkAssociations">The link associations.</param>
        /// <param name="parentId">The parent identifier.</param>
		public void UpdateBulk(IEnumerable<LinkAssociation> linkAssociations, Guid parentId)
		{
            linkAssociations.ToList().ForEach(x => SetupLinkAssociation(_principalResolver, x));
			_linkAssociationRepository.UpdateBulk(linkAssociations, parentId);
		}

		/// <summary>
		/// Setups the task.
		/// </summary>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="linkAssociation">The task.</param>
		internal static void SetupLinkAssociation(IPrincipalResolver principalResolver, LinkAssociation linkAssociation)
		{
			var currentDateTime = DateTime.UtcNow;
			linkAssociation.CreatedById = principalResolver.UserId;
			linkAssociation.CreatedDateTime = currentDateTime;
			linkAssociation.UpdatedById = principalResolver.UserId;
			linkAssociation.UpdatedDateTime = currentDateTime;
		}

	}
}
