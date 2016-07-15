using Aspose.Cells;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Builds validations for value types in workbooks.
    /// </summary>
    public class AllowedValuteTypeValidationBuilder : WorkbookValidationBuilderBase
    {
        /// <summary>
        ///     Adds the validation values.
        /// </summary>
        /// <param name="validationWorksheet">The validation worksheet.</param>
        /// <returns></returns>
        protected override Range AddValidationValues(Worksheet validationWorksheet)
        {
            int labelColum = ExcelTemplateKeys.AllowedValueTypeValidatorColumn;
            validationWorksheet.Cells[0, labelColum].PutValue("Allowed Value Types");
            validationWorksheet.Cells[1, labelColum].PutValue("Name");

            int currentRow = 2;

            validationWorksheet.Cells[currentRow, labelColum].PutValue("Single");
            ++currentRow;
            validationWorksheet.Cells[currentRow, labelColum].PutValue("Multiple");
            ++currentRow;
            validationWorksheet.Cells[currentRow, labelColum].PutValue("Range");
            ++currentRow;
            validationWorksheet.Cells[currentRow, labelColum].PutValue("Multiple, Range");


            Range valueRange = validationWorksheet.Cells.CreateRange(2, labelColum, 4, 1);
            // Name the range.
            valueRange.Name = ExcelTemplateKeys.AllowedValueTypeLabel.Replace(" ", "");
            return valueRange;
        }
    }
}