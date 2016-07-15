using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Builds document section for Miscellaneous
    /// </summary>
    public class MiscellaneousProjectDocumentSectionBuilder:IProjectDocumentSectionBuilder
    {
        /// <summary>
        /// Adds the section.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="project">The project.</param>
        /// <param name="startRow">The start row.</param>
        /// <returns>Last Row Populated</returns>
        public int AddSection(Workbook workbook, Worksheet worksheet, Project project, int startRow)
        {
            var currentRow = startRow;
            worksheet.Cells[currentRow, 0].Value = "Miscellaneous";
            ++currentRow;
            worksheet.Cells[currentRow, 0].Value = "Additional Criteria";
            worksheet.Cells[currentRow, 1].Value = project.AdditionalCriteria;
            worksheet.Cells[currentRow, 2].Value = "Industry";
            worksheet.Cells[currentRow, 3].Value = project.Industry;
            ++currentRow;
            worksheet.Cells[currentRow, 0].Value = "Industry Category";
            worksheet.Cells[currentRow, 1].Value = project.IndustryCategory;
            worksheet.Cells[currentRow, 2].Value = "Industry Sub-Category";
            worksheet.Cells[currentRow, 3].Value = project.IndustrySubcategory;
            ++currentRow;
            worksheet.Cells[currentRow, 0].Value = "Location";
            worksheet.Cells[currentRow, 1].Value = project.Location;
            worksheet.Cells[currentRow, 2].Value = "Product Group";
            worksheet.Cells[currentRow, 3].Value = project.ProductGroup;
            ++currentRow;
            worksheet.Cells[currentRow, 0].Value = "Description";
            worksheet.Cells[currentRow, 1].Value = project.Description;
            return currentRow;
        }
    }
}
