using System.Globalization;

using Aspose.Cells;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// 
    /// </summary>
    public class EstimatesAndConstraintsInformationProjectDocumentSectionBuilder : IProjectDocumentSectionBuilder
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
            worksheet.Cells[currentRow, 0].PutValue("Estimates & Constraints");
            worksheet.Cells[currentRow, 0].SetProjectHeadingStyle();
            ++currentRow;
            
            var lastFilledRow = worksheet.Cells[currentRow, 0]
                .StartTwoPlusTwoColumn("Completion Date", project.CompletionDate.HasValue ? project.CompletionDate.Value.ToShortDateString() : "")
                .AddTwoPlusTwoColumn("Estimated TAT Date", project.EstimatedTATDate.HasValue ? project.EstimatedTATDate.Value.ToShortDateString() : "")
                .AddTwoPlusTwoColumn("Days in Current Phase", project.DaysInCurrentPhase.ToString(CultureInfo.InvariantCulture))
                .AddTwoPlusTwoColumn("Estimated Engineering Effort (hrs)", project.EstimateEngineeringEffort.HasValue? project.EstimateEngineeringEffort.Value.ToString(CultureInfo.InvariantCulture) : "")
                .AddTwoPlusTwoColumn("Estimated Lab Efforts (hrs)", project.EstimatedLabEffort.HasValue? project.EstimatedLabEffort.Value.ToString(CultureInfo.InvariantCulture): "")
                .AddTwoPlusTwoColumn("Estimated Reviewer Effort (hrs)", project.EstimatedReviewerEffort.HasValue ? project.EstimatedReviewerEffort.Value.ToString(CultureInfo.InvariantCulture) : "")
                .AddTwoPlusTwoColumn("Scope", project.Scope ?? "")
                .AddTwoPlusTwoColumn("Assumptions", project.Assumptions ?? "")
                .AddTwoPlusTwoColumn("Engineering Office Limitation", project.EngineeringOfficeLimitations ?? "")
                .AddTwoPlusTwoColumn("Laboratory Limitation", project.LaboratoryLimitations ?? "")
                .AddTwoPlusTwoColumn("Complexity", project.Complexity ?? "")
                ;

            // todo : products
            return lastFilledRow.Row;
        }
    }
}