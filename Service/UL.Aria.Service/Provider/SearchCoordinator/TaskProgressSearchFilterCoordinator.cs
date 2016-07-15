using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Provider.SearchCoordinator.Task;

namespace UL.Aria.Service.Provider.SearchCoordinator
{
	/// <summary>
	/// 
	/// </summary>
	[SearchCoordinatorFor(EntityTypeEnumDto.Task, Ordinal = 1)]
	public class TaskProgressSearchFilterCoordinator : ISearchCoordinator
	{
		private readonly ITaskProgressQueryBuilderFactory _taskProgressQueryBuilderFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskProgressSearchFilterCoordinator"/> class.
		/// </summary>
		/// <param name="taskProgressQueryBuilderFactory">The task progress query builder factory.</param>
		public TaskProgressSearchFilterCoordinator(ITaskProgressQueryBuilderFactory taskProgressQueryBuilderFactory)
		{
			_taskProgressQueryBuilderFactory = taskProgressQueryBuilderFactory;
		}
	
		private static readonly string _exclusionText = string.Format("{0}AND(NOT({1}), NOT({2}))",
	   AssetFieldNames.NoQuotes, TaskStatusEnumDto.Completed.ToString(), TaskStatusEnumDto.Canceled.ToString());
		private readonly Dictionary<string, List<string>> _existingFilter = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> _filter = new Dictionary<string, List<string>>();
		/// <summary>
		/// When implemented in derived classes applies adjustments to the search criteria object
		/// to coordinate pre SharePoint Query changes.
		/// </summary>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public Domain.Search.SearchCriteria ApplyCoordination(Domain.Search.SearchCriteria criteria)
		{

			if (criteria.Filters.Keys.Any(x => x.Contains(AssetFieldNames.AriaTaskProgressCal)))
			{
				_existingFilter.Add(AssetFieldNames.AriaTaskProgressCal, criteria.Filters[AssetFieldNames.AriaTaskProgressCal]);
				criteria.Filters.Remove(AssetFieldNames.AriaTaskProgressCal);
				var filtervalues = _existingFilter[AssetFieldNames.AriaTaskProgressCal];
				var dependencytKey = AssetFieldNames.AriaTaskPhaseLabel.ToString();
				
				var stringBuilder = new StringBuilder();
				
				if (filtervalues.Count > 0)
					stringBuilder.Append("AND(");

				_filter = new Dictionary<string, List<string>>();
				_filter.Add(dependencytKey, new List<string>());
				_filter[dependencytKey].Add(_exclusionText);

				stringBuilder.Append(SharePointQuery.BuildRefinementFilters(_filter));

				if (filtervalues.Count > 0)
					stringBuilder.Append(",");

				if (filtervalues.Count > 1)
				{
					stringBuilder.Append("OR(");
				}

				foreach (var fvalue in filtervalues)
				{
					string query = _taskProgressQueryBuilderFactory.GetQueryBuilder(fvalue).BuildRefinerFilterQuery();
					stringBuilder.Append(query);
					stringBuilder.Append(",");
				}

				stringBuilder.Length--;
				if (filtervalues.Count > 1)
					stringBuilder.Append(")");

				if (filtervalues.Count > 0)
					stringBuilder.Append(")");
				
				criteria.AdditionalFilterString = stringBuilder.ToString();
				criteria.SearchCoordinators.Add(this);
				return criteria;
			}
			return criteria;
		}

		/// <summary>
		/// When implemented in derived classes removes all adjustments to the search criteria object
		/// made to coordinate pre SharePoint Query changes during an ApplyCoordination method call.
		/// This is the reverse function to ApplyCoordination.
		/// </summary>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public Domain.Search.SearchCriteria RetractCoordination(Domain.Search.SearchCriteria criteria)
		{
			foreach (var filter in _existingFilter)
			{
				criteria.Filters.Add(filter.Key, filter.Value);
			}
			criteria.AdditionalFilterString = string.Empty;
			criteria.SearchCoordinators.Remove(this);
			return criteria;
		}
	}
}
