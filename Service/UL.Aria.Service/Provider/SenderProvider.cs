using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class SenderProvider.
    /// </summary>
    public class SenderProvider : ISenderProvider
    {
        private readonly ISenderRepository _senderRepository;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SenderProvider" /> class.
        /// </summary>
        /// <param name="senderRepository">The sender repository.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public SenderProvider(ISenderRepository senderRepository, ITransactionFactory transactionFactory)
        {
            _senderRepository = senderRepository;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Creates the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public void Create(Sender sender)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                _senderRepository.Create(sender);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Finds the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Sender.</returns>
        public Sender FindByName(string name)
        {
            return _senderRepository.FetchByName(name);
        }
    }
}