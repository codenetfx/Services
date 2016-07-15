using System.Linq;

using Aspose.Cells;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Builds project document section for Order Informaiton
    /// </summary>
    public class OrderInformationProjectDocumentSectionBuilder : IProjectDocumentSectionBuilder
    {
        /// <summary>
        ///     Adds the section.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="project">The project.</param>
        /// <param name="startRow">The start row.</param>
        /// <returns>Last Row Populated</returns>
        public int AddSection(Workbook workbook, Worksheet worksheet, Project project, int startRow)
        {
            var currentRow = startRow;
            ++currentRow;
            worksheet.Cells[currentRow, 0].PutValue("Order Information");
            worksheet.Cells[currentRow, 0].SetProjectHeadingStyle();
            ++currentRow;
            var lastFilledCell = worksheet.Cells[currentRow, 0];
            lastFilledCell = lastFilledCell
                .StartTwoPlusTwoColumn("Order Number", project.OrderNumber)
                .AddTwoPlusTwoColumn("Type", project.OrderType)
                .AddTwoPlusTwoColumn("Status", project.Status)
                .AddTwoPlusTwoColumn("") //padding
                .AddTwoPlusTwoColumn("Business Unit", project.BusinessUnit)
                .AddTwoPlusTwoColumn("Customer PO", project.CustomerPo)
                .AddTwoPlusTwoColumn("Created",
					project.CreationDate.HasValue ? project.CreationDate.Value.ToShortDateString() : "")
                .AddTwoPlusTwoColumn("Updated",
                    project.LastUpdateDate.HasValue ? project.LastUpdateDate.Value.ToShortDateString() : "")
				.AddTwoPlusTwoColumn("Ordered", 
					project.DateOrdered.HasValue ? project.DateOrdered.Value.ToShortDateString() : "")
                .AddTwoPlusTwoColumn("Booked",
					project.DateBooked.HasValue ? project.DateBooked.Value.ToShortDateString() : "")
                .AddTwoPlusTwoColumn("Inventory Item/Catalog Nos.", project.InventoryItemCatalogNumbers ?? "")
                .AddTwoPlusTwoColumn("Inventory Item Nos. Descriptions.", project.InventoryItemNumbersDescriptions ?? "")
                .AddTwoPlusTwoColumn("Quote No.", project.QuoteNo ?? "")
                .AddTwoPlusTwoColumn("Expedited", project.Expedited ? "Yes" : "No")
                .AddTwoPlusTwoColumn("Price", project.Price ?? "")
                .AddTwoPlusTwoColumn("Standards", project.Standards ?? "")
                .AddTwoPlusTwoColumn("Project Type", project.ProjectType ?? "")
                .AddTwoPlusTwoColumn("Service Description", project.ServiceDescription ?? "")
                ;
            currentRow = lastFilledCell.Row;
            worksheet.Cells[currentRow, 0].PutValue("Service Line Information");
            worksheet.Cells[currentRow, 0].SetProjectHeadingStyle();
            if (null == project.ServiceLines || !project.ServiceLines.Any())
            {
                return currentRow;
            }
            ++currentRow;

            lastFilledCell = worksheet.Cells[currentRow, 0];
            lastFilledCell
                .AddTableHeader("Line Number", true)
                .AddTableHeader("Service Line")
                .AddTableHeader("Service Program")
                .AddTableHeader("Service Category")
                .AddTableHeader("Service Sub-category")
                .AddTableHeader("Service Segment")
                .AddTableHeader("Service Description")
                .AddTableHeader("Additional Charges Allowed")
                .AddTableHeader("Allow Cross Charge?")
                .AddTableHeader("Billable?")
                .AddTableHeader("Client Detailed Service")
                .AddTableHeader("Promise Date")
                .AddTableHeader("Request Date")
                .AddTableHeader("Status")
                .AddTableHeader("Type")
                ;
            ++currentRow;
            foreach (var serviceLine in project.ServiceLines)
            {
                lastFilledCell = worksheet.Cells[currentRow, 0];
                lastFilledCell
                    .AddTableValue(serviceLine.LineNumber, true)
                    .AddTableValue(serviceLine.Name)
                    .AddTableValue(serviceLine.Program)
                    .AddTableValue(serviceLine.ItemCategories)
                    .AddTableValue(serviceLine.SubCategory)
                    .AddTableValue(serviceLine.Segment)
                    .AddTableValue(serviceLine.Description)
                    .AddTableValue(serviceLine.BillableExpenses)
                    .AddTableValue(serviceLine.AllowChargesFromOtherOperatingUnits)
                    .AddTableValue(serviceLine.Billable)
                    .AddTableValue(serviceLine.ClientDetailService)
                    .AddTableValue(serviceLine.PromiseDate.ToShortDateString())
                    .AddTableValue(serviceLine.RequestDate.ToShortDateString())
                    .AddTableValue(serviceLine.StartDate.ToShortDateString())
                    .AddTableValue(serviceLine.Status)
                    .AddTableValue(serviceLine.TypeCode)
                    ;
            }


            return lastFilledCell.Row;
        }
    }
}