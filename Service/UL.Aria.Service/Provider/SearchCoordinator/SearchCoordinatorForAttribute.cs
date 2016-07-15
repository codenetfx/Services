using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider.SearchCoordinator
{
    /// <summary>
    /// Indicator of the entity type that the coordinator is applicable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class SearchCoordinatorForAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCoordinatorForAttribute"/> class.
        /// </summary>
        /// <param name="entityType"> The type of the entity being search to coordinate.</param>
        public SearchCoordinatorForAttribute(EntityTypeEnumDto entityType)
        {
            this.EntityType = entityType;
        }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity being search to coordinate.
        /// </value>
        public EntityTypeEnumDto EntityType { get; set; }
        
        /// <summary>
        /// Gets or sets the sort ordinal.
        /// </summary>
        /// <value>
        /// The ordinal.
        /// </value>
        public int Ordinal { get; set; }
    }
}
