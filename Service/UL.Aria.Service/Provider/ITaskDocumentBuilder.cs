using System.Collections.Generic;
using System.IO;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations to build download documents for tasks.
    /// </summary>
    public interface ITaskDocumentBuilder
    {
        /// <summary>
        /// Builds a documetn for the specified tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        Stream Build(IEnumerable<Domain.Entity.Task> tasks);

        /// <summary>
        /// Builds the specified tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        Stream Build(IEnumerable<Domain.Entity.TaskProjectMapping> tasks);
    }
}