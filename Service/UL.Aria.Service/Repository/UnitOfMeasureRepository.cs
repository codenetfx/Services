using System.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for <see cref="UnitOfMeasure" />
    /// </summary>
    public class UnitOfMeasureRepository : TrackedDomainEntityRepositoryBase<UnitOfMeasure>, IUnitOfMeasureRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfMeasureRepository" /> class.
        /// </summary>
        public UnitOfMeasureRepository() : base("UnitOfMeasureId", "UnitOfMeasure")
        {
        }

        /// <summary>
        ///     Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">
        ///     if set to <c>true</c> [is new].
        /// </param>
        /// <param name="isDirty">
        ///     if set to <c>true</c> [is dirty].
        /// </param>
        /// <param name="isDelete">
        ///     if set to <c>true</c> [is delete].
        /// </param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(UnitOfMeasure entity, bool isNew, bool isDirty, bool isDelete,
                                                  DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            dr["UnitOfMeasureName"] = entity.Name;
            dr["UnitOfMeasureDescription"] = entity.Description;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override UnitOfMeasure ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.Name = reader.GetValue<string>("UnitOfMeasureName");
            entity.Description = reader.GetValue<string>("UnitOfMeasureDescription");

            return entity;
        }
    }
}