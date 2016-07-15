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
	[QueryBuilder(TaskProgressEnumDto.Slipping)]
	public class TaskProgressSlippingQueryBuilder : QueryBuilderBase
	{
		/// <summary>
		/// Builds the refiner filter query.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		
		public override string BuildRefinerFilterQuery()
		{

			var filter = new Dictionary<string, List<string>>
			{
				{AssetFieldNames.AriaTaskDueDate, new List<string>()},
				{AssetFieldNames.AriaTaskReminderDate, new List<string>()}
			};

			filter[AssetFieldNames.AriaTaskReminderDate].Add(string.Format(RangeFormat, "min",
				DateTime.UtcNow.ToString(SpDateFormat)));

			filter[AssetFieldNames.AriaTaskDueDate].Add(string.Format(NotRangeFormat, "min",
				DateTime.UtcNow.AddDays(-1).ToString(SpDateFormat)));
			return SharePointQuery.BuildRefinementFilters(filter);
		}
	}
}
