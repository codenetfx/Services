using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Provides Link data services.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
    InstanceContextMode = InstanceContextMode.PerCall)]
    public class LinkService : ILinkService
    {
        private readonly ILinkManager _linkManager;
        private readonly IMapperRegistry _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkService" /> class.
        /// </summary>
        /// <param name="linkManager">The link manager.</param>
        /// <param name="mapper">The mapper.</param>
        public LinkService(ILinkManager linkManager, IMapperRegistry mapper)
        {
            _linkManager = linkManager;
            _mapper = mapper;
        }


        /// <summary>
        /// Fetches the link matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public LinkDto FetchById(string id)
        {
            Guard.IsNotNullOrEmptyTrimmed(id, "id");
            return _mapper.Map<LinkDto>(_linkManager.FetchById(Guid.Parse(id)));
        }

        /// <summary>
        /// Fetches all active links associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<LinkDto> FetchLinksByEntity(string entityId)
        {
            Guard.IsNotNullOrEmptyTrimmed(entityId, "id");
            return _mapper.Map<List<LinkDto>>(_linkManager.FetchLinksByEntity(Guid.Parse(entityId)));
        }

        /// <summary>
        /// Deletes a link with the specified linkId.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(string linkId)
        {
            Guard.IsNotNullOrEmptyTrimmed(linkId, "linkId");
            _linkManager.Delete(Guid.Parse(linkId));
        }

        /// <summary>
        /// Creates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public LinkDto Create(LinkDto link)
        {
            Guard.IsNotNull(link, "link");
            var entity = _linkManager.Create(_mapper.Map<Link>(link));
            return _mapper.Map<LinkDto>(entity);
        }

        /// <summary>
        /// Updates the specified link.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>
        /// <param name="link">The link.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(string linkId, LinkDto link)
        {
            Guard.IsNotNullOrEmptyTrimmed(linkId, "linkId");
            Guard.IsNotNull(link, "link");
            var entity = _mapper.Map<Link>(link);
            _linkManager.Update(entity);

        }

        /// <summary>
        /// Searches the specified search criteria dto.
        /// </summary>
        /// <param name="searchCriteriaDto">The search criteria dto.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public LinkSearchDto Search(SearchCriteriaDto searchCriteriaDto)
        {
            Guard.IsNotNull(searchCriteriaDto, "searchCriteriaDto");
            var criteria = _mapper.Map<SearchCriteria>(searchCriteriaDto);
            var searchResultSet = _linkManager.Search(criteria);
            return _mapper.Map<LinkSearchDto>(searchResultSet);
        }

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<LookupDto> GetLookups()
        {
            return _mapper.Map<List<LookupDto>>(_linkManager.GetLookups());
        }
    }
}
