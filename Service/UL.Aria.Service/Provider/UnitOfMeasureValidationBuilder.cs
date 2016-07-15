using Aspose.Cells;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for injecting unit of measure validators into worksheet.
    /// </summary>
    public class UnitOfMeasureValidationBuilder : WorkbookValidationBuilderBase
    {
        private readonly IUnitOfMeasureProvider _unitOfMeasureProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfMeasureValidationBuilder" /> class.
        /// </summary>
        /// <param name="unitOfMeasureProvider">The unit of measure provider.</param>
        public UnitOfMeasureValidationBuilder(IUnitOfMeasureProvider unitOfMeasureProvider)
        {
            _unitOfMeasureProvider = unitOfMeasureProvider;
        }


        /// <summary>
        /// Adds the validation values.
        /// </summary>
        /// <param name="validationWorksheet">The validation worksheet.</param>
        /// <returns></returns>
        protected override Range AddValidationValues(Worksheet validationWorksheet)
        {
            int idColumn = ExcelTemplateKeys.UnitOfMeasureIdColumn;
            int labelColum = ExcelTemplateKeys.UnitOfMeasureLabelColum;
            validationWorksheet.Cells[0, idColumn].PutValue("Unit of Measure");
            validationWorksheet.Cells.Merge(0, idColumn, 1, 2);
            validationWorksheet.Cells[1, idColumn].PutValue("Id");
            validationWorksheet.Cells[1, labelColum].PutValue("Name");

            var currentRow = 2;
            foreach (var unitOfMeasure in _unitOfMeasureProvider.GetAll())
            {
                validationWorksheet.Cells[currentRow, idColumn].PutValue(unitOfMeasure.Id.Value.ToString());
                validationWorksheet.Cells[currentRow, labelColum].PutValue(unitOfMeasure.Name);
                ++currentRow;
            }
            
            // ++ at the end, so it is -2.
            Range valueRange = validationWorksheet.Cells.CreateRange(2, idColumn, currentRow - 2, 1);
            // Name the range.
            valueRange.Name = ExcelTemplateKeys.UoMLabel.Replace(" ", "");
            return valueRange;
        }
    }
}
