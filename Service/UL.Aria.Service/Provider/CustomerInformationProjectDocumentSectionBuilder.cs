using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerInformationProjectDocumentSectionBuilder : IProjectDocumentSectionBuilder
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
            worksheet.Cells[currentRow, 0].PutValue("Customer Information");
            worksheet.Cells[currentRow, 0].SetProjectHeadingStyle();
            ++currentRow;
            var incomingOrderContact = project.IncomingOrderContact ?? new IncomingOrderContact();
            var incomingOrderCustomer = project.IncomingOrderCustomer ?? new IncomingOrderCustomer();
            var lastFilledRow = worksheet.Cells[currentRow, 0]
                .StartTwoPlusTwoColumn("Contact", incomingOrderContact.FullName)
                .AddTwoPlusTwoColumn("Contact Title", "")
                .AddTwoPlusTwoColumn("Customer", incomingOrderCustomer.Name)
                .AddTwoPlusTwoColumn("Address", "")
                .AddTwoPlusTwoColumn("ContactPhone", "")
                .AddTwoPlusTwoColumn("State", "")
                .AddTwoPlusTwoColumn("Email", "")
                .AddTwoPlusTwoColumn("Country", "")
                .AddTwoPlusTwoColumn("Subscriber Number", "")
                .AddTwoPlusTwoColumn("Agent Details", "")
                .AddTwoPlusTwoColumn("Customer Project Name", incomingOrderCustomer.ProjectName)

                ;

            // todo : products
            return lastFilledRow.Row;
        }
    }
}
