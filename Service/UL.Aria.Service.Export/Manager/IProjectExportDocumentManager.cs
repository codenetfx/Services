using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    /// Defines operations for exporting <see cref="Project"/>s to a file stream.
    /// </summary>
    public interface IProjectExportDocumentManager
    {
        /// <summary>
        /// Exports <see cref="Project" />s to the supplied stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="projectIds"></param>
        /// <param name="companies">The companies.</param>
        /// <param name="projectLookupFunc">The project lookup function.</param>
        void ExportProjects(Stream stream, IEnumerable<Guid> projectIds, Dictionary<Guid, Company> companies, Func<Guid, Project> projectLookupFunc);
        
        /// <summary>
        /// Creates the header in the export document for the project.
        /// </summary>
        /// <param name="stream"></param>
        void CreateProjectHeader(Stream stream);

        /// <summary>
        /// Exports <see cref="Task" />s to the supplied stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="projectDetail"></param>
        void ExportProjectTasks(Stream stream, ProjectDetail projectDetail);

        /// <summary>
        /// Creates the headers in the export document for the tasks.
        /// </summary>
        /// <param name="stream"></param>
        void CreateTaskHeader(Stream stream);
    }
}