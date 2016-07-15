using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Product family attribute repository class.
    /// </summary>
    public class ProductFamilyAttributeRepository : CharacteristicRepositoryBase<ProductFamilyAttribute>,IProductFamilyAttributeRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyAttributeRepository" /> class.
        /// </summary>
        public ProductFamilyAttributeRepository()
            : base("Attribute")
        {
            
        }



        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(ProductFamilyAttribute entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);  

            dr["DataTypeId"] = (byte)entity.DataTypeId;
            if (entity.UnitOfMeasureId.HasValue)
                dr["UnitOfMeasureId"] = entity.UnitOfMeasureId;
            else
                dr["UnitOfMeasureId"] = DBNull.Value;
        }


        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyAttribute ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.DataTypeId = (ProductFamilyCharacteristicDataType) reader.GetValue<byte>("DataTypeId");
            entity.UnitOfMeasureId = reader.GetValue<Guid?>("UnitOfMeasureId");
            return entity;
        }

        /// <summary>
        /// Finds the attributes by id list.
        /// </summary>
        /// <param name="productFamilyIds">The product family ids.</param>
        /// <returns></returns>
        public IList<ProductFamilyAttribute> FindByIds(IList<Guid> productFamilyIds)
        {
            //This will probably need to be refactored to more performant later
            return productFamilyIds.Select(FindById)
                .ToList();
        }

        /// <summary>
        /// Removes the option.
        /// </summary>
        /// <param name="attributeId">The attribute unique identifier.</param>
        /// <param name="optionId">The option unique identifier.</param>
        public void RemoveOption(Guid attributeId, Guid optionId)
        {
        }
    }
}