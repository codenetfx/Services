using System.Globalization;

using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Builds project document section for Order Information
    /// </summary>
    public class GeneralInformationProjectDocumentSectionBuilder : IProjectDocumentSectionBuilder
    {
        /// <summary>
        /// Adds the section.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="project">The project.</param>
        /// <param name="startRow">The start row.</param>
        /// <returns>the last row populated by this builder.</returns>
        public int AddSection(Workbook workbook, Worksheet worksheet, Project project, int startRow)
        {
            var currentRow = startRow;
            ++currentRow;
            worksheet.Cells[currentRow, 0].PutValue("General Information");
            worksheet.Cells[currentRow, 0].SetProjectHeadingStyle();
            ++currentRow;
            var lastFilledRow = worksheet.Cells[currentRow, 0]
                .StartTwoPlusTwoColumn("Project Name", project.Name)
                .AddTwoPlusTwoColumn("Project Number", project.ProjectNumber)
                .AddTwoPlusTwoColumn("Project Type", project.Type.ToString())
                .AddTwoPlusTwoColumn("Project Handler", project.ProjectHandler)
                .AddTwoPlusTwoColumn("Start Date", project.StartDate.HasValue? project.StartDate.Value.ToShortDateString(): "")
                .AddTwoPlusTwoColumn("End Date", project.EndDate.HasValue ? project.EndDate.Value.ToShortDateString() : "")
                .AddTwoPlusTwoColumn("Project Status", project.ProjectStatus.ToString())
                .AddTwoPlusTwoColumn("Description", project.Description)
                .AddTwoPlusTwoColumn("Number of Samples", project.NumberOfSamples.HasValue ? project.NumberOfSamples.Value.ToString(CultureInfo.InvariantCulture) : "")
                .AddTwoPlusTwoColumn("Sample Reference Numbers", project.SampleReferenceNumbers ?? "")
                .AddTwoPlusTwoColumn("CCN", project.CCN)
                .AddTwoPlusTwoColumn("File No.", project.FileNo)
                .AddTwoPlusTwoColumn("Status Notes", project.StatusNotes)
                ;

            // todo : products
            return lastFilledRow.Row;
        }
    }
}
