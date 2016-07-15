using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Defines operations for working with <see cref="TaskProperty" /> objects.
    /// </summary>
    public interface ITaskPropertyProvider : ISearchProviderBase<TaskProperty>
    {
        /// <summary>
        ///     Finds the by task property type identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="taskPropertyTypeId">The task property type identifier.</param>
        /// <returns></returns>
        IList<T> FindByTaskPropertyTypeId<T>(Guid taskId, Guid taskPropertyTypeId) where T : TaskProperty, new();

        /// <summary>
        ///     Finds the by task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IList<TaskProperty> FindByTaskId(Guid taskId);
    }
}