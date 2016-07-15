using System;
using System.Drawing;
using System.Globalization;
using Aspose.Cells;

namespace UL.Aria.Service.Provider
{
    internal static class WorkbookExtensions
    {
        public static Row Hide(this Row row)
        {
            row.IsHidden = true;
            return row;
        }

        public static Row Lock(this Row row)
        {
            row.Style.IsLocked = true;
            return row;
        }

        public static Column Hide(this Column column)
        {
            column.IsHidden = true;
            return column;
        }

        public static Column Lock(this Column columm)
        {
            columm.Style.IsLocked = true;
            return columm;
        }

        public static string CharacteristicType(this Cell cell)
        {
            return cell.Worksheet.Cells[ExcelTemplateKeys.CharacteristicTypeRow, cell.Column].StringValue;
        }

        public static string CharacteristicFamilyId(this Cell cell)
        {
            return cell.Worksheet.Cells[ExcelTemplateKeys.CharacteristicIdRow, cell.Column].StringValue;
        }

        public static bool TryGetProductCharacteristicId(this Cell cell, out Guid guid)
        {
            guid = Guid.Empty;
            var familyCharacteristicId = cell.Worksheet.Cells[ExcelTemplateKeys.CharacteristicIdRow, cell.Column].StringValue;
            
            //var columnIndex = cell.Column - 1;
            var columnIndex = cell.Worksheet.FindCharacteristicIdColumn(familyCharacteristicId);
            if (0 >= columnIndex)
                return false;
            if (ExcelTemplateKeys.IdIdentifier !=
                cell.Worksheet.Cells[ExcelTemplateKeys.CharacteristicIdRow, columnIndex].StringValue)
                return false;
            var id =
                cell.Worksheet.Cells[cell.Row, columnIndex]
                    .StringValue;
            return Guid.TryParse(id, out guid);
        }

        public static int FindCharacteristicIdColumn(this Worksheet worksheet, string familyCharacteristicId)
        {
            var idCell = worksheet.Cells.Find(familyCharacteristicId, worksheet.Cells[ExcelTemplateKeys.CharacteristicTypeRow, 0], new FindOptions { SeachOrderByRows = false, LookInType = LookInType.Values });
            var idColumnIndex = idCell.Column;
            return idColumnIndex;
        }

        /// <summary>
        /// Adds the label and value two column.
        /// </summary>
        /// <param name="lastCell">The last cell.</param>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Cell AddTwoPlusTwoColumn(this Cell lastCell, string label, string value = "")
        {
            var row = lastCell.Row;
            var column = lastCell.Column;
            Cell cell = 1 == column ? lastCell.Worksheet.Cells[row, 2] : lastCell.Worksheet.Cells[++row, 0];
            cell.PutValue(label);
            cell = cell.Worksheet.Cells[row, cell.Column + 1];
            cell.PutValue(value);
            return cell;
        }

        /// <summary>
        /// Adds the label and value two column.
        /// </summary>
        /// <param name="lastCell">The last cell.</param>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        /// <param name="isFirst">if set to <c>true</c> [is first].</param>
        /// <returns></returns>
        public static Cell AddTwoColumn(this Cell lastCell, string label, string value = "", bool isFirst = false)
        {
            
            int row = isFirst ? lastCell.Row : lastCell.Row + 1;
            const int column = 0;
            var cell = lastCell.Worksheet.Cells[row, column];
            cell.PutValue(label);
            cell = cell.Worksheet.Cells[row, column + 1];
            cell.PutValue(value);
            return cell;
        }

        /// <summary>
        /// Starts the label and value two plus two column.
        /// </summary>
        /// <param name="startCell">The start cell.</param>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Cell StartTwoPlusTwoColumn(this Cell startCell, string label, string value = "")
        {
            Cell cell = startCell;
           
            cell.PutValue(label);
            cell = cell.Worksheet.Cells[cell.Row, cell.Column + 1];
            cell.PutValue(value);
            return cell;
        }

        public static Cell AddTableHeader(this Cell lastCell, string label, bool isFirst = false)
        {
            Cell cell = isFirst ? lastCell : lastCell.Worksheet.Cells[lastCell.Row, lastCell.Column + 1];
            cell.PutValue(label);
            cell.SetProjectHeadingStyle();
            return cell;
        }

        public static void SetProjectHeadingStyle(this Cell cell)
        {
            var style = new Style();
            style.Font.IsBold = true;
            style.BackgroundColor = Color.CornflowerBlue;
            cell.SetStyle(style);
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="lastCell">The last cell.</param>
        /// <param name="label">The label.</param>
        /// <param name="isFirst">if set to <c>true</c> [is first].</param>
        /// <returns></returns>
        public static Cell AddTableValue(this Cell lastCell, string label, bool isFirst = false)
        {
            Cell cell = isFirst ? lastCell : lastCell.Worksheet.Cells[lastCell.Row, lastCell.Column + 1];
            cell.PutValue(label);
            return cell;
        }

        public static Cell AddTableValue(this Cell lastCell, object label, bool isFirst = false)
        {
            Cell cell = isFirst ? lastCell : lastCell.Worksheet.Cells[lastCell.Row, lastCell.Column + 1];
            cell.PutValue(label);
            return cell;
        }

        public static Cell AddTableDateValueNullAsEmpty(this Cell lastCell, DateTime? label, bool isFirst = false)
        {
            Cell cell = isFirst ? lastCell : lastCell.Worksheet.Cells[lastCell.Row, lastCell.Column + 1];
            cell.PutValue(label.HasValue ? label.Value.ToShortDateString() : string.Empty);
            return cell;
        }

        public static Cell AddTableIntValueNullAsEmpty(this Cell lastCell, int? label, bool isFirst = false)
        {
            Cell cell = isFirst ? lastCell : lastCell.Worksheet.Cells[lastCell.Row, lastCell.Column + 1];
            cell.PutValue(label.HasValue ? label.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
            return cell;
        }

        public static Cell AddTableDecimalValueNullAsEmpty(this Cell lastCell, Decimal? label, bool isFirst = false)
        {
            Cell cell = isFirst ? lastCell : lastCell.Worksheet.Cells[lastCell.Row, lastCell.Column + 1];
            cell.PutValue(label.HasValue ? label.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
            return cell;
        }

        /// <summary>
        /// To the yes no.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static string ToYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }

        /// <summary>
        /// Froms the yes no.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool FromYesNo(this string value)
        {
            return value.ToLowerInvariant() == "yes";
        }

        /// <summary>
        /// Finds the last filled row.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <returns></returns>
        public static int FindLastFilledRow(this Worksheet worksheet)
        {
            Cell lastCell = worksheet.Cells.LastCell;
            if (null == lastCell)
                return 1;
            var row = lastCell.Row;
            return row;
        }
    }
}