using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Search;
using System;

namespace UL.Aria.Service.Repository
{
   // [Obsolete("Use methods in SearchRepositoryBase. this class needs refactored out and removed.", false)]
    internal static class RepositoryHelper
    {
        internal static void AddFilter(Database db, DbCommand cmd, SearchCriteria searchCriteria, string assetFieldName,
            string parameterName)
        {
            if (searchCriteria.Filters.ContainsKey(assetFieldName))
            {
                var command = cmd as SqlCommand;
                SqlParameter parameter = command.CreateParameter();
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.ParameterName = parameterName;
                parameter.Value = ConvertToDataTable(searchCriteria.Filters[assetFieldName]);
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);
                //db.AddInParameter(command, parameterName, SqlDbType.Structured,
                //    searchCriteria.Filters[assetFieldName].First());
            }
        }

        internal static DataTable ConvertToDataTable(IList<string> filterValues)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("FilterValue");
            foreach (string filterValue in filterValues)
            {
                dataTable.Rows.Add(filterValue);
            }
            return dataTable;
        }
    }
}