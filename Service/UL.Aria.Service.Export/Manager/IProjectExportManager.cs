using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    /// Defines operations for exporting <see cref="Project"/> entities. fs
    /// </summary>
    public interface IProjectExportManager
    {
        /// <summary>
        /// Exports the projects.
        /// </summary>
        void ExportProjects();
    }
}