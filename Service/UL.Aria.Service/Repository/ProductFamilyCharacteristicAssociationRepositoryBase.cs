using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Base class for shared characteristic repository functionality.
    /// </summary>
    public abstract class ProductFamilyCharacteristicAssociationRepositoryBase<T> : TrackedDomainEntityRepositoryBase<T> where T : ProductFamilyCharacteristicAssociation, new()
    {
        private readonly string _optiondIdFieldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyCharacteristicAssociationRepositoryBase{T}" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the database identifier field.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="optiondIdFieldName">Name of the optiond identifier field.</param>
        protected ProductFamilyCharacteristicAssociationRepositoryBase(string dbIdFieldName, string tableName, string optiondIdFieldName) : base(dbIdFieldName, tableName)
        {
            _optiondIdFieldName = optiondIdFieldName;
        }

        /// <summary>
        ///     Initializes the save command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <param name="saveEnum">The save enum.</param>
        /// <returns></returns>
        protected override DbCommand InitializeSaveCommand(T entity, Database db, SaveEnum saveEnum)
        {
            var command= base.InitializeSaveCommand(entity, db, saveEnum);
            var dataTable = new DataTable();
            dataTable.Columns.Add("FirstForeignKey", typeof(Guid));
            dataTable.Columns.Add("SecondForeignKey", typeof(Guid));
            foreach (var optionId in entity.OptionIds)
            {
                var dataRow = dataTable.NewRow();
                dataRow["FirstForeignKey"] = entity.Id.Value;
                dataRow["SecondForeignKey"] = optionId;
                dataTable.Rows.Add(dataRow);
            }

            var sqlParameter = new SqlParameter("@OptionList", SqlDbType.Structured) { Value = dataTable };
            command.Parameters.Add(sqlParameter);
            return command;
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<T> FindAll()
        {
            return ExecuteReaderCommand(database => InitializeFindCommand(Guid.Empty, database));
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override T FindById(Guid entityId)
        {
            var result = ExecuteReaderCommand(database => InitializeFindCommand(entityId, database));

            if (result.Count == 0)
                throw new DatabaseItemNotFoundException();
            return result.First<T>();
        }

        /// <summary>
        /// Executes the reader command.
        /// </summary>
        /// <param name="InitializeCommandDelegate">The initialize command delegate.</param>
        /// <returns></returns>
        protected IList<T> ExecuteReaderCommand(Func<Database, DbCommand> InitializeCommandDelegate)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeCommandDelegate.Invoke(database))
            {
                var result = new Dictionary<Guid, T>();

                using (IDataReader reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        var productFamilyAttributeAssociation = ConstructEntity(reader);
                        result.Add(productFamilyAttributeAssociation.Id.Value, productFamilyAttributeAssociation);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        var associationId = reader.GetValue<Guid>(IdFieldName);
                        var optionId = reader.GetValue<Guid>(_optiondIdFieldName);
                        result[associationId].OptionIds.Add(optionId);
                    }
                }
                return result.Values.ToList();
            }
        }
    }
}