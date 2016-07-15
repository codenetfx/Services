using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider.SearchCoordinator.Task
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class QueryBuilderBase : IQueryBuilder
	{
		internal const string SpDateFormat = "yyyy-MM-dd";
		internal const string RangeFormat = AssetFieldNames.NoQuotes + "range({0}, {1})";
		internal const string NotRangeFormat = AssetFieldNames.NoQuotes + "(NOT(range({0}, {1})))";

		/// <summary>
		/// Builds the refiner filter query.
		/// </summary>
		/// <returns></returns>
		public abstract string BuildRefinerFilterQuery();
	}
}
