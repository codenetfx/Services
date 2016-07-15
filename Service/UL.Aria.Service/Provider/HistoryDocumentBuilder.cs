using Aspose.Cells;
using Aspose.Cells.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Builds download documents for history.
    /// </summary>
    public class HistoryDocumentBuilder : IHistoryDocumentBuilder
    {
	    private readonly IProfileRepository _profileRepository;

	    /// <summary>
		/// Initializes a new instance of the <see cref="HistoryDocumentBuilder"/> class.
		/// </summary>
		/// <param name="profileRepository">The profile repository.</param>
	    public HistoryDocumentBuilder(IProfileRepository profileRepository)
		{
			_profileRepository = profileRepository;
		}

	    /// <summary>
        /// Builds a document for the specified entity histories.
        /// </summary>
        /// <param name="histories">The entity histories.</param>
        /// <returns></returns>
        public Stream Build(IEnumerable<History> histories)
        {
            var workbook = new Workbook();
            workbook.Worksheets.Clear();
			Build(workbook, histories);

            Stream stream = null;
            try
            {
                stream = new MemoryStream();
                workbook.Save(stream, SaveFormat.Xlsx);
                stream.Position = 0;
                return stream;
            }
            catch (Exception)
            {
                using (stream) { } // dispose if there's a failure, otherwise, we're returning it!
                throw;
            }
        }

        private void FinalizeStyle(Workbook workbook, int lastColumn)
        {
            
            var sheet = workbook.Worksheets[0];
            
            var headerStyle = new Style();
            headerStyle.Font.Name = "Calibri";
            headerStyle.Font.Size = 10;
            headerStyle.SetTwoColorGradient(Color.FromArgb(177, 8, 32), Color.FromArgb(142, 5, 24), GradientStyleType.Horizontal, 1);
            headerStyle.Font.Color = Color.White;
            var headerStyleFlag = new StyleFlag { FontName = true, FontSize = true, FontColor = true, CellShading = true };
            
            var style = new Style();
            style.Font.Name = "Calibri";
            style.Font.Size = 10;
            style.IsTextWrapped = true;
            style.VerticalAlignment = TextAlignmentType.Top;
            var styleFlag = new StyleFlag { WrapText = true, FontName = true, FontSize = true, VerticalAlignment = true};
            
            sheet.Cells.ApplyStyle(style, styleFlag);
            var range = sheet.Cells.CreateRange(0, 0, 1, lastColumn);

            range.ApplyStyle(headerStyle, headerStyleFlag);

            sheet.AutoFitColumns();
        }

        internal void Build(Workbook workbook, IEnumerable<History> histories)
        {
            var worksheet = workbook.Worksheets.Add(ExcelTemplateKeys.Histories);
            var lastCell = AddBasicHeaders(worksheet);
            histories.Aggregate(lastCell, (current, history) => AddHistory(history, current, 0));
            FinalizeStyle(workbook,14);
        }

        private static Cell AddBasicHeaders(Worksheet worksheet, bool isFirst = true)
        {
            var lastCell = worksheet.Cells[0, 0]
                .AddTableHeader("Date/Time UTC", isFirst)
                .AddTableHeader("Event")
                .AddTableHeader("Details")
                .AddTableHeader("Updated By")
                ;
            return lastCell;
        }

        private Cell AddHistory(History history, Cell lastCell, int indent)
        {
            var cell = lastCell.Worksheet.Cells[lastCell.Row + 1, 0];
			cell = AddHistoryColumns(history, indent, true, cell);
            return cell;
        }

        private Cell AddHistoryColumns(History history, int indent, bool isFirst, Cell cell)
        {
	        string displayName = string.Empty;
	        if (history.ActionUserId != Guid.Empty)
	        {
				var profile = _profileRepository.FetchById(history.ActionUserId);
		        if (profile != null)
			        displayName = profile.DisplayName;
	        }
	        return cell
				.AddTableValue(history.ActionDate + string.Empty, isFirst)
                .AddTableValue(history.ActionType + string.Empty)
                .AddTableValue(history.ActionDetail + string.Empty)
				.AddTableValue(displayName + string.Empty);
        }
    }
}
