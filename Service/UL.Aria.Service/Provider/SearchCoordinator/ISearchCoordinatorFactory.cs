using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider.SearchCoordinator
{
    /// <summary>
    /// Supplies a Factory interface for retrieving Search Coordinators
    /// </summary>
    public interface ISearchCoordinatorFactory
    {
        /// <summary>
        /// Gets a list of coordinators registered to the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        List<ISearchCoordinator> GetCoordinators(EntityTypeEnumDto entityType);
    }
}
