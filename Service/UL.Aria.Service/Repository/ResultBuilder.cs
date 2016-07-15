using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.PeerResolvers;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Linq;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Builds results for filtered and sorted result sets.
    /// </summary>
    internal class ResultBuilder
    {
        /// <summary>
        /// Sorts the specified results.
        /// </summary>
        /// <param name="sorts">The sorts.</param>
        /// <param name="resultsToSort">The results to sort.</param>
        /// <returns>Sorted list</returns>
        public static IList<T> Sort<T>(IList<ISort> sorts, IEnumerable<T> resultsToSort)
        {
            var resultsToSortList = resultsToSort as IList<T> ?? resultsToSort.ToList();
            if (sorts.Count == 0)
            {
                return resultsToSortList.ToList();
            }

            IOrderedQueryable<T> orderedQueryable = null;
            var primarySort = sorts[0];
            if (primarySort.Order.Equals(SortDirection.Ascending))
            {
                orderedQueryable = resultsToSortList.OrderBy(primarySort.FieldName);
            }
            else
            {
                orderedQueryable = resultsToSortList.OrderByDescending(primarySort.FieldName);
            }

            for (int i = 1; i < sorts.Count; i++)
            {
                var secondarySort = sorts[i];
                if (secondarySort.Order.Equals(SortDirection.Ascending))
                {
                    orderedQueryable = orderedQueryable.ThenBy(secondarySort.FieldName);
                }
                else
                {
                    orderedQueryable = orderedQueryable.ThenByDescending(secondarySort.FieldName);
                }
            }

            return orderedQueryable.ToList();
        }

        /// <summary>
        /// Pages the specified results to page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultsToPage">The results to page.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        /// <returns>Single paged result set.</returns>
        public static IList<T> Page<T>(IEnumerable<T> resultsToPage, int startIndex, int endIndex)
        {
            var resultsToPageList = resultsToPage as IList<T> ?? resultsToPage.ToList();
            if (resultsToPageList.Count() > ((endIndex - startIndex) + 1))
            {
                var pagedResult = new List<T>();

                pagedResult = resultsToPageList
                    .Skip(startIndex)
                    .Take((endIndex - startIndex) + 1)
                    .ToList();

                return pagedResult;

            }
            return resultsToPageList;
        }
    }
}