using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Base repository for options.
    /// </summary>
    public class ProductCharacteristicOptionRepositoryBase : TrackedDomainEntityRepositoryBase<ProductFamilyCharacteristicOption>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCharacteristicOptionRepositoryBase"/> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the db id field.</param>
        /// <param name="tableName">Name of the table.</param>
        protected ProductCharacteristicOptionRepositoryBase(string dbIdFieldName, string tableName) : base(dbIdFieldName, tableName)
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
        protected override void AddTableRowFields(ProductFamilyCharacteristicOption entity, bool isNew, bool isDirty, bool isDelete, System.Data.DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr[TableName.Replace("Option", "") + "Id"] = entity.ProductFamilyCharacteristicId;
            dr[TableName + "Name"] = entity.Name;
            dr[TableName + "Description"] = entity.Description;
            dr[TableName + "Value"] = entity.Value;
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyCharacteristicOption ConstructEntity(System.Data.IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);
            entity.Name = reader.GetValue<string>(TableName + "Name");
            entity.Description = reader.GetValue<string>(TableName + "Description");
            entity.Value = reader.GetValue<string>(TableName + "Value");
            return entity;
        }

        /// <summary>
        /// Fills the option.
        /// </summary>
        /// <typeparam name="TCharacteristic">The type of the characteristic.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="attributes">The attributes.</param>
        public void FillOption<TCharacteristic>(IDataReader reader, IList<TCharacteristic> attributes) where TCharacteristic:ProductFamilyCharacteristicDomainEntity
        {
            var option = ConstructEntity(reader);

            var attribute = attributes.FirstOrDefault(x => x.Id.Value == option.ProductFamilyCharacteristicId);
            if (null == attribute)
                return;
            attribute.Options.Add(option);
        }
    }
}