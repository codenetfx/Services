using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Link Provider implementation
    /// </summary>
    public class LinkProvider:ILinkProvider
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IBusinessUnitProvider _businessUnitProvider;
        private readonly ILinkAssociationProvider _linkAssociationProvider;
	    private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkProvider" /> class.
        /// </summary>
        /// <param name="linkRepository">The link repository.</param>
        /// <param name="businessUnitProvider">The business unit provider.</param>
        /// <param name="linkAssociationProvider">The link association provider.</param>
        /// <param name="principalResolver">The principal resolver.</param>
	    public LinkProvider(ILinkRepository linkRepository, IBusinessUnitProvider businessUnitProvider, ILinkAssociationProvider linkAssociationProvider, IPrincipalResolver principalResolver)
        {
            _linkRepository = linkRepository;
            _businessUnitProvider = businessUnitProvider;
            _linkAssociationProvider = linkAssociationProvider;
		    _principalResolver = principalResolver;
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>        
        public Link FetchById(Guid id)
        {
            var link = _linkRepository.FindById(id);
            link.BusinessUnits = _businessUnitProvider.FetchGroup(id);
            return link;
        }

        /// <summary>
        /// Fetches all active links associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>      
        public IEnumerable<Link> FetchLinksByEntity(Guid entityId)
        {
            return _linkRepository.FetchLinksByEntity(entityId);
        }

        /// <summary>
        /// Deletes a link with the specified linkId.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>
        public void Delete(Guid linkId)
        {
            _linkRepository.Remove(linkId);
        }

        /// <summary>
        /// Creates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>        
        public Link Create(Link link)
        {
			SetupLink(_principalResolver, link);
            link.UpdatedDateTime = link.CreatedDateTime;
             _linkRepository.Add(link);
             _businessUnitProvider.UpdateBulk(link.BusinessUnits, link.Id.Value);
             return link;
        }

        /// <summary>
        /// Updates the link associations.
        /// </summary>
        /// <param name="links">The links.</param>
        /// <param name="parentId">The parent identifier.</param>
        public void UpdateLinkAssociations(IEnumerable<Link> links, Guid parentId)
	    {
            var i = 0;
			var currentDateTime = DateTime.UtcNow;
            var associations = links.Select(x => new LinkAssociation()
            {
	            LinkId = x.Id.Value,
	            ParentId = parentId,
	            Order = ++i,
	            CreatedById = _principalResolver.UserId,
	            CreatedDateTime = currentDateTime,
	            UpdatedById = _principalResolver.UserId,
	            UpdatedDateTime = currentDateTime,
            }).ToList();

            _linkAssociationProvider.UpdateBulk(associations, parentId);
	    }

        /// <summary>
        /// Updates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>        
        public void Update(Link link)
        {
			SetupLink(_principalResolver, link);
            _linkRepository.Update(link);
            _businessUnitProvider.UpdateBulk(link.BusinessUnits, link.Id.Value);
        }

        /// <summary>
        /// Searches the specified search criteria dto.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public ISearchResultSet<Link> Search(SearchCriteria searchCriteria)
        {
            return _linkRepository.Search(searchCriteria);
        }

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>     
        public IEnumerable<Lookup> GetLookups()
        {
            return _linkRepository.GetLookups();
        }

		internal static void SetupLink(IPrincipalResolver principalResolver, Link link)
		{
			if (!link.Id.HasValue)
			{
				link.Id = Guid.NewGuid();
			}
			link.CreatedById = principalResolver.UserId;
			link.UpdatedById = principalResolver.UserId;
		}
    }
}
