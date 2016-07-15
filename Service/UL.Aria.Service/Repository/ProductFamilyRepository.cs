using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Class ProductFamilyRepository
    /// </summary>
    public class ProductFamilyRepository : TrackedDomainEntityRepositoryBase<ProductFamily>, IProductFamilyRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyRepository"/> class.
        /// </summary>
        public ProductFamilyRepository() : base("FamilyId", "Family")
        {
        }

        /// <summary>
        /// Gets the product familes by business unit.
        /// </summary>
        /// <param name="businessUnitId">The business unit id.</param>
        /// <returns>
        /// IReadOnlyDictionary{GuidProductFamily}.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IReadOnlyDictionary<Guid, ProductFamily> GetProductFamiliesByBusinessUnit(Guid businessUnitId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(ProductFamily entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);      

            dr["FamilyName"] = entity.Name;
            dr["FamilyDescription"] = entity.Description;
            dr["AllowChanges"] = entity.AllowChanges;
            dr["IsDisabled"] = entity.IsDisabled;
            if (entity.BusinessUnitId.HasValue)
            dr["BusinessUnitId"] = entity.BusinessUnitId;
            else
            {
                dr["BusinessUnitId"] = DBNull.Value;
            }
            dr["CategoryId"] = entity.CategoryId;
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamily ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.Name = reader.GetValue<string>("FamilyName");
            entity.Description = reader.GetValue<string>("FamilyDescription");
            entity.AllowChanges = reader.GetValue<bool>("AllowChanges");
            entity.IsDisabled = reader.GetValue<bool>("IsDisabled");
            entity.BusinessUnitId = reader.GetValue<Guid?>("BusinessUnitId");
            entity.CategoryId = reader.GetValue<Guid>("CategoryId");
            return entity;
        }
    }
}