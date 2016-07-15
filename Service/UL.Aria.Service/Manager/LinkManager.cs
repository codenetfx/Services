using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Link manager implementation
    /// </summary>
    public class LinkManager : ILinkManager
    {
        private readonly ILinkProvider _linkProvider;   
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkManager" /> class.
        /// </summary>
        /// <param name="linkProvider">The link provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public LinkManager(ILinkProvider linkProvider, ITransactionFactory transactionFactory)
        {
            _linkProvider = linkProvider;          
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Fetches the link matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Link FetchById(Guid id)
        {
            return _linkProvider.FetchById(id);
        }

        /// <summary>
        /// Fetches all active links associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Link> FetchLinksByEntity(Guid entityId)
        {
            return _linkProvider.FetchLinksByEntity(entityId);        
        }

        /// <summary>
        /// Deletes a link with the specified linkId.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid linkId)
        {
            using (var transaction = _transactionFactory.Create())
            {
                _linkProvider.Delete(linkId);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Creates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Link Create(Link link)
        {           
            using (var transaction = _transactionFactory.Create())
            {
                 link =  _linkProvider.Create(link);                
                transaction.Complete();
            }

            return link;
        }

        /// <summary>
        /// Updates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(Link link)
        {
         
            using (var transaction = _transactionFactory.Create())
            {
                _linkProvider.Update(link);
                transaction.Complete();
            }        
        }

        /// <summary>
        /// Searches the specified search criteria dto.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ISearchResultSet<Link> Search(SearchCriteria criteria)
        {
            var searchResultSet = _linkProvider.Search(criteria);
            return  searchResultSet;
        }

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>   
        public IEnumerable<Lookup> GetLookups()
        {
            return _linkProvider.GetLookups();         
        }
    }
}
