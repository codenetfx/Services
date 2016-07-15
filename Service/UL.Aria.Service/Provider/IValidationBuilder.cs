using Aspose.Cells;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for injecting unit of measure validators into worksheet.
    /// </summary>
    public interface IValidationBuilder
    {
        /// <summary>
        /// Builds the specified workbook.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="columnToValidate">The column to validate.</param>
        /// <param name="maxRows"></param>
        void Build(Workbook workbook, int columnToValidate, int maxRows);

    }
}