using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using UL.Aria.Service.Domain.Lookup;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Lookup Repository
    /// </summary>
    public class LookupRepository :  ILookupRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LookupRepository" /> class.
        /// </summary>
        public LookupRepository()
        {
           
        }
        
        /// <summary>
        ///     Fetch all businessUnits
        /// </summary>
        /// <returns>IList{Lookup}.</returns>
        public IEnumerable<BusinessUnit> FetchAllBusinessUnits()
        {
            IList<BusinessUnit> businessUnitList;
           
                var db = DatabaseFactory.CreateDatabase();
                using (var command = InitializeFetchAllBusinessUnits(db))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        businessUnitList = ConstructCompleteBusinessUnitsList(reader);
                    }
                }
                return businessUnitList;
        }

        private static DbCommand InitializeFetchAllBusinessUnits(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pBusinessUnit_GetAll]");
            return command;
        }

        private static IList<BusinessUnit> ConstructCompleteBusinessUnitsList(IDataReader reader)
        {
            var businessUnitList = new List<BusinessUnit>();
            while (reader.Read())
            {
                var businessUnit = ConstructBusinessUnit(reader);
                businessUnitList.Add(businessUnit);
            }
            return businessUnitList;
        }

        private static BusinessUnit ConstructBusinessUnit(IDataReader reader)
        {
            return new BusinessUnit
            {
                Id = reader.GetValue<Guid>("BusinessUnitId"),
                Name = reader.GetValue<string>("BusinessUnitName"),
                Code = reader.GetValue<string>("BusinessUnitCode")
            };
        }


        /// <summary>
        /// Fetches the business unit by entity.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        public IEnumerable<BusinessUnit> FetchBusinessUnitByEntity(Guid parentId)
        {
            var businessUnits = new List<BusinessUnit>();
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeGetBusinessUnitByEntityCommand(db, parentId))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        var link = ConstructBusinessUnit(reader);
                        businessUnits.Add(link);
                    }
                }
            }

            return businessUnits;
        }

        private static DbCommand InitializeGetBusinessUnitByEntityCommand(Database db, Guid parentId)
        {
            var cmd = db.GetStoredProcCommand("[dbo].[pBusinessUnit_GetByEntity]");
            var param = cmd.CreateParameter();
            param.ParameterName = "ParentId";
            param.DbType = DbType.Guid;
            param.Value = parentId;
            cmd.Parameters.Add(param);
            return cmd;
        }
    }
}
