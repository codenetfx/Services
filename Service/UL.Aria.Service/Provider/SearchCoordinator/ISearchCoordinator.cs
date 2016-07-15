using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider.SearchCoordinator
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISearchCoordinator
    {

        /// <summary>
        /// When implemented in derived classes applies adjustments to the search criteria object 
        /// to coordinate pre SharePoint Query changes.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
      SearchCriteria ApplyCoordination(SearchCriteria criteria);


      /// <summary>
      /// When implemented in derived classes removes all adjustments to the search criteria object 
      /// made to coordinate pre SharePoint Query changes during an ApplyCoordination method call.
      /// This is the reverse function to ApplyCoordination.
      /// </summary>
      /// <param name="criteria">The criteria.</param>
      /// <returns></returns>
      SearchCriteria RetractCoordination(SearchCriteria criteria);

    }
}
