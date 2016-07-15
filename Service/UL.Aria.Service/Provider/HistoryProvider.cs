using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///  History Provider
    /// </summary>
    public class HistoryProvider : IHistoryProvider
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HistoryProvider" /> class.
        /// </summary>
        /// <param name="historyRepository">The project repository.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public HistoryProvider(IHistoryRepository historyRepository,
            ITransactionFactory transactionFactory, IPrincipalResolver principalResolver)
        {
            _historyRepository = historyRepository;
            _transactionFactory = transactionFactory;
            _principalResolver = principalResolver;
        }

        /// <summary>
        ///     Fetches all history items for a given Entity, i.e. Project History, Order History, etc
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>IEnumerable{History}.</returns>
        public IEnumerable<History> FetchHistoryByEntityId(Guid entityId)
        {
            return _historyRepository.FindAllByEntityId(entityId);
        }

        /// <summary>
        ///     Creates the specified history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>HistoryId.</returns>
        public Guid Create(History history)
        {
            var createdDateTime = DateTime.UtcNow;
            var createdBy = _principalResolver.UserId;
            history.ActionDate = createdDateTime;
            history.ActionUserId = createdBy;

            using (var transactionScope = _transactionFactory.Create())
            {
                _historyRepository.Create(history);
                // ReSharper disable once PossibleInvalidOperationException
                transactionScope.Complete();
            }

            return history.HistoryId;
        }
    }
}
