using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface ILinkAssociationProvider
	/// </summary>
	public interface ILinkAssociationProvider
	{
        /// <summary>
        /// Updates the link associations.
        /// </summary>
        /// <param name="linkAssociations">The link associations.</param>
        /// <param name="parentId">The parent identifier.</param>
		void UpdateBulk(IEnumerable<LinkAssociation> linkAssociations, Guid parentId);
	}
}
