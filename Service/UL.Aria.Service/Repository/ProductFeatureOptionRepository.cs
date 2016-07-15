using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{

    /// <summary>
    /// Defines repository for Feature Options
    /// </summary>
    public interface IProductFeatureOptionRepository : IRepositoryBase<ProductFamilyCharacteristicOption>, IProductCharacteristicOptionRepository
    {
    }

    /// <summary>
    /// Repository for Feature Options
    /// </summary>
    public class ProductFeatureOptionRepository : ProductCharacteristicOptionRepositoryBase, IProductFeatureOptionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFeatureOptionRepository" /> class.
        /// </summary>
        public ProductFeatureOptionRepository()
            : base("FeatureOptionId", "FeatureOption")
        {

        }
    }
}