using System;
using System.Collections.Generic;
using System.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Product characteristic repository
    /// </summary>
    public interface IProductCharacteristicRepository : IRepositoryBase<ProductCharacteristic>
    {
        /// <summary>
        ///     Finds the by product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        IList<ProductCharacteristic> FindByProductId(Guid? productId);

        /// <summary>
        ///     Deletes the children.
        /// </summary>
        /// <param name="id">The id.</param>
        void DeleteChildren(Guid id);

        /// <summary>
        /// Constructs the product characteristic.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        ProductCharacteristic ConstructProductCharacteristic(IDataReader reader);
    }
}