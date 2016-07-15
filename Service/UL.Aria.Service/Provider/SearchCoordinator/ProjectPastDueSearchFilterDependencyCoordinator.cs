using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;


namespace UL.Aria.Service.Provider.SearchCoordinator
{

    /// <summary>
    /// Provide methods for appling and retracting past due project status requirments 
    /// </summary>
    [SearchCoordinatorFor(EntityTypeEnumDto.Project, Ordinal = 1)]
    public class ProjectPastDueSearchFilterDependencyCoordinator : ISearchCoordinator
    {
      private static readonly string _exclusionText = string.Format("{0}AND(NOT({1}), NOT({2}))",
        AssetFieldNames.NoQuotes, ProjectStatusEnumDto.Completed.ToString(), ProjectStatusEnumDto.Canceled.ToString());
        private List<string> _existingStatuses = new List<string>();
        private string[] _assuredStatuses = new string[] { ProjectStatusEnumDto.Completed.ToString(), ProjectStatusEnumDto.Canceled.ToString() };

        /// <summary>
        /// When implemented in derived classes applies adjustments to the search criteria object 
        /// to coordinate pre SharePoint Query changes.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public SearchCriteria ApplyCoordination(SearchCriteria criteria)
        {
            foreach (var key in criteria.Filters.Keys)
            {
                if (criteria.Filters[key].Any(x => x.Contains(AssetFieldNames.PastDue)))
                {
                    var dependencytKey = AssetFieldNames.AriaProjectProjectStatus.ToString();
                    if (!criteria.Filters.ContainsKey(dependencytKey))
                        criteria.Filters.Add(dependencytKey, new List<string>());

                    foreach (var status in _assuredStatuses)
                    {
                        var pos = criteria.Filters[dependencytKey].IndexOf(status);

                        if (pos >= 0)
                        {
                            criteria.Filters[dependencytKey].RemoveAt(pos);
                            _existingStatuses.Add(status);
                        }
                    }

                    criteria.Filters[dependencytKey].Add(_exclusionText);
                    criteria.SearchCoordinators.Add(this);
                    return criteria;
                }
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
        public SearchCriteria RetractCoordination(SearchCriteria criteria)
        {
            var dependencytKey = AssetFieldNames.AriaProjectProjectStatus.ToString();

            if (criteria.Filters.ContainsKey(dependencytKey))
            {
                criteria.Filters[dependencytKey].Remove(_exclusionText);
            }

            foreach (var status in _existingStatuses)
            {
                if (!criteria.Filters[dependencytKey].Exists(x => x == status))
                {
                    criteria.Filters[dependencytKey].Add(status);
                }
            }

            if (criteria.Filters.ContainsKey(dependencytKey)
                && criteria.Filters[dependencytKey].Count <= 0)
                criteria.Filters.Remove(dependencytKey);

            criteria.SearchCoordinators.Remove(this);
            return criteria;
        }
    }
}
