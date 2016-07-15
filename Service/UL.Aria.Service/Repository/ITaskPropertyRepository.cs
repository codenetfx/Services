using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines repository operations for working with <see cref="TaskProperty"/> objects.
    /// </summary>
    public interface ITaskPropertyRepository : IPrimaryEntityRepository<TaskProperty>
    {

        /// <summary>
        /// Finds the by task property type identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="taskPropertyTypeId">The task property type identifier.</param>
        /// <returns></returns>
        IList<TaskProperty> FindByTaskPropertyTypeId(Guid taskId, Guid taskPropertyTypeId);

        /// <summary>
        /// Finds the by task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IList<TaskProperty> FindByTaskId(Guid taskId);
    }
}