using Aspose.Cells;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Base class for <see cref="IValidationBuilder"/> for workbooks.
    /// </summary>
    public abstract class WorkbookValidationBuilderBase : IValidationBuilder
    {
        /// <summary>
        /// Builds the specified workbook.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="columnToValidate">The column to validate.</param>
        /// <param name="maxRows"></param>
        public void Build(Workbook workbook, int columnToValidate, int maxRows)
        {
            var validationWorksheet = workbook.Worksheets[ExcelTemplateKeys.ValidationsLabel] ?? workbook.Worksheets.Add(ExcelTemplateKeys.ValidationsLabel);
            var valueRange = AddValidationValues(validationWorksheet);
            AddValidation(workbook, columnToValidate, maxRows, valueRange);
            validationWorksheet.AutoFitColumns();
        }

        /// <summary>
        /// Adds the validation values.
        /// </summary>
        /// <param name="validationWorksheet">The validation worksheet.</param>
        /// <returns></returns>
        protected abstract Range AddValidationValues(Worksheet validationWorksheet);

        /// <summary>
        /// Adds the validation.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="columnToValidate">The column to validate.</param>
        /// <param name="maxRows">The max rows.</param>
        /// <param name="valueRange">The value range.</param>
        protected virtual void AddValidation(Workbook workbook, int columnToValidate, int maxRows, Range valueRange)
        {
            var worksheet = workbook.Worksheets[ExcelTemplateKeys.AttributesAndFeatures];
            var validationCollection = worksheet.Validations;
            var validationIndex = validationCollection.Add();
            var validation = validationCollection[validationIndex];
            validation.Type = ValidationType.List;
            validation.Operator = OperatorType.None;
            validation.InCellDropDown = true;
            validation.Formula1 = "=" + valueRange.Name;
            validation.ShowError = true;
            validation.AlertStyle = ValidationAlertType.Stop;

            var area = new CellArea();
            area.StartRow = 1;
            area.StartColumn = columnToValidate;
            area.EndColumn = columnToValidate;
            area.EndRow = maxRows;

            validation.AreaList.Add(area);
        }
    }
}