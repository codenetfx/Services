using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for building documents from <see cref="Project"/> objects.
    /// </summary>
    public interface IProjectDocumentBuilder
    {
        /// <summary>
        /// Builds the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        Stream Build(Project project);
    }
}
