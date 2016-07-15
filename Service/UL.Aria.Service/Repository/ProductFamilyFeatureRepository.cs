using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Persists <see cref="ProductFamilyFeature" /> entities in the database.
    /// </summary>
    public class ProductFamilyFeatureRepository : CharacteristicRepositoryBase<ProductFamilyFeature>, IProductFamilyFeatureRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyFeatureRepository" /> class.
        /// </summary>
        public ProductFamilyFeatureRepository() : base("Feature")
        {
        }



        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyFeature ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.AllowChanges = reader.GetValue<bool>("AllowChanges");

            return entity;
        }


        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(ProductFamilyFeature entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);      
            
            dr["AllowChanges"] = entity.AllowChanges;
        }
        
        /// <summary>
        /// Finds the features by id list.
        /// </summary>
        /// <param name="productFamilyIds">The product family ids.</param>
        /// <returns></returns>
        public IList<ProductFamilyFeature> FindByIds(IList<Guid> productFamilyIds)
        {
            return null;
        }
    }
}