using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class EntityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAttribute"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityAttribute(EntityTypeEnumDto entity)
        {
            EntityType = entity;
        }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public EntityTypeEnumDto EntityType { get; private set; }
    }
}
