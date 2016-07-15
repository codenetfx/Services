using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for building documents from <see cref="Project"/> objects.
    /// </summary>
    public class ProjectDocumentBuilder : IProjectDocumentBuilder
    {
        private readonly IEnumerable<IProjectDocumentSectionBuilder> _sectionBuilders;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDocumentBuilder"/> class.
        /// </summary>
        public ProjectDocumentBuilder()
        {
            _sectionBuilders=new List<IProjectDocumentSectionBuilder>
                {
                    new GeneralInformationProjectDocumentSectionBuilder(),
                    new CustomerInformationProjectDocumentSectionBuilder(),
                    new EstimatesAndConstraintsInformationProjectDocumentSectionBuilder(),
                    new OrderInformationProjectDocumentSectionBuilder(),
                    new MiscellaneousProjectDocumentSectionBuilder()
                };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDocumentBuilder"/> class.
        /// </summary>
        /// <param name="sectionBuilders">The section builders.</param>
        internal ProjectDocumentBuilder(IEnumerable<IProjectDocumentSectionBuilder> sectionBuilders)
        {
            _sectionBuilders = sectionBuilders;
        }

        /// <summary>
        /// Builds the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public Stream Build(Project project)
        {
            var workbook = new Workbook();
            workbook.Worksheets.Clear();
            var worksheet = workbook.Worksheets.Add(project.Name);
            var row = -1;
            foreach (var sectionBuilder in _sectionBuilders)
            {
                row = sectionBuilder.AddSection(workbook, worksheet, project, ++row);
            }
            worksheet.AutoFitColumns();
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
    }
}