using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Builds document downloads for several Projects.
    /// </summary>
    public interface IMultiProjectDocumentBuilder
    {
        /// <summary>
        /// Builds the specified project.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns></returns>
        Stream Build(IEnumerable<ProjectDetail> projects);
    }
}