using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for building sections in documents from <see cref="Project"/> objects.
    /// </summary>
    public interface IProjectDocumentSectionBuilder
    {
        /// <summary>
        /// Adds the section.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="project">The project.</param>
        /// <param name="startRow">The start row.</param>
        /// <returns>
        /// the last row populated by this builder.
        /// </returns>
        int AddSection(Workbook workbook, Worksheet worksheet, Project project, int startRow);
    }
}