using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Relay.Manager;

namespace UL.Aria.Service.Relay.Service
{
    /// <summary>
    /// 
    /// </summary>
        [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.PerCall,
        Namespace = @"http://aria.ul.com/Relay/ProductDetail"
        )]
    public class RelayProductService : IRelayProductService
    {
        private readonly IRelayProductManager _relayProductManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProductService"/> class.
        /// </summary>
        /// <param name="relayProductManager">The relay product manager.</param>
        public RelayProductService(IRelayProductManager relayProductManager)
        {
            _relayProductManager = relayProductManager;
        }

        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProductDto GetProductById(string id)
        {
            System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");

            return _relayProductManager.GetProductById(convertedId);
        }
    }
}
