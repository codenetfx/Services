using System;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRelayProductManager
    {
        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ProductDto GetProductById(Guid id);
    }
}