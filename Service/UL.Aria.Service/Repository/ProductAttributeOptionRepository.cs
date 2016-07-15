using System.Collections.Generic;
using System.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines repository for options
    /// </summary>
    public interface IProductCharacteristicOptionRepository
    {
        /// <summary>
        /// Fills the option.
        /// </summary>
        /// <typeparam name="TCharacteristic">The type of the characteristic.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="attributes">The attributes.</param>
        void FillOption<TCharacteristic>(IDataReader reader, IList<TCharacteristic> attributes) where TCharacteristic : ProductFamilyCharacteristicDomainEntity;
    }

    /// <summary>
    /// Repository for AttributeOptions
    /// </summary>
    public class ProductAttributeOptionRepository : ProductCharacteristicOptionRepositoryBase, IProductAttributeOptionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAttributeOptionRepository" /> class.
        /// </summary>
        public ProductAttributeOptionRepository()
            : base("AttributeOptionId", "AttributeOption")
        {
            
        }
    }
}