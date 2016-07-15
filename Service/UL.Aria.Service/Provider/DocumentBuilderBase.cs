using System;
using System.IO;
using Aspose.Cells;
using UL.Aria.Common;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Base implementation for classes which build documents.
    /// </summary>
    public abstract class DocumentBuilderBase
    {
        /// <summary>
        ///     Initializes the document and makes it ready for writing.
        ///     This
        /// </summary>
        public Workbook InitializeDocument()
        {
            var workbook = InitializeDocument(new MemoryStream());
            workbook.Worksheets.Clear();
            return workbook;
        }

        /// <summary>
        ///     Initializes the document based on an existing <see cref="Stream" /> and makes it ready for writing.
        /// </summary>
        public Workbook InitializeDocument(Stream stream)
        {
            var workbook = new Workbook(stream);
            return workbook;
        }

        /// <summary>
        /// Adds the worksheet.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns></returns>
        protected Worksheet AddWorksheet(Workbook workbook, string sheetName)
        {
            var substring = sheetName.Trim().Substring(0, Math.Min(sheetName.Length, 30));
            return workbook.Worksheets.Add(substring);
        }

        /// <summary>
        /// Finds the worksheet.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns></returns>
       protected Worksheet FindWorksheet(Workbook workbook, string sheetName)
        {
            var substring = sheetName.Trim().Substring(0, Math.Min(sheetName.Length, 30));
            return workbook.Worksheets[substring];
        }

        /// <summary>
        ///     Closes the document.
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns>The stream with the document.</returns>
        public Stream FinalizeDocument(Workbook workbook)
        {
            FinalizeStyle(workbook);
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
                using (stream){} // dispose if there's a failure, otherwise, we're returning it!
                throw;
            }
        }

        /// <summary>
        /// Finalizes the style.
        /// </summary>
        /// <param name="workbook"></param>
        protected abstract void FinalizeStyle(Workbook workbook);
    }
}