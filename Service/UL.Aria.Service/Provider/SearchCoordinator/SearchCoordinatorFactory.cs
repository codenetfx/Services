using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using System.Reflection;
using System.Collections.Concurrent;
using UL.Aria.Service.Provider.SearchCoordinator.Task;

namespace UL.Aria.Service.Provider.SearchCoordinator
{
    /// <summary>
    /// Default Search Coordinator Factory.
    /// </summary>
    public class SearchCoordinatorFactory : ISearchCoordinatorFactory
    {
        private readonly IUnityContainer _container;
        private Dictionary<EntityTypeEnumDto, List<OrderedCoordinator>> _lookup = new Dictionary<EntityTypeEnumDto, List<OrderedCoordinator>>();
        private static readonly object pad_lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCoordinatorFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SearchCoordinatorFactory(IUnityContainer container)
        {
            _container = container;
            InitLookup();
        }

        /// <summary>
        /// Initializes the lookup.
        /// </summary>
        private void InitLookup()
        {

            lock (pad_lock)
            {
                var coordinators = _container.ResolveAll<ISearchCoordinator>();

                foreach (var item in coordinators)
                {
                    var itemType = item.GetType();
                    var attr = itemType.GetCustomAttributes<SearchCoordinatorForAttribute>().FirstOrDefault();
                    if (attr != null)
                    {
                        if (!this._lookup.ContainsKey(attr.EntityType))
                        {
                            this._lookup.Add(attr.EntityType, new List<OrderedCoordinator>());
                        }

                        if (null == this._lookup[attr.EntityType].FirstOrDefault(x => x.ResolutionType == itemType))
                        {
                            this._lookup[attr.EntityType].Add(new OrderedCoordinator()
                            {
                                Ordinal = attr.Ordinal,
                                ResolutionType = itemType
                            });

                        }
                    }
                }
                

                var keys = _lookup.Keys.ToList();

                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    _lookup[key] = _lookup[key].OrderBy(x => x.Ordinal).ToList();
                }
            }
        }

        /// <summary>
        /// Gets a list of coordinators registered to the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public List<ISearchCoordinator> GetCoordinators(EntityTypeEnumDto entityType)
        {
            var coordinators = new List<ISearchCoordinator>();
            if (this._lookup.ContainsKey(entityType))
            {

                var neededTypes = this._lookup[entityType];
                foreach (var neededType in neededTypes)
                {
	                coordinators.Add((ISearchCoordinator) _container.Resolve(neededType.ResolutionType));
                }

            }

            return coordinators;
        }


        private class OrderedCoordinator
        {
            public int Ordinal { get; set; }
            public Type ResolutionType { get; set; }
        }

    }
}
