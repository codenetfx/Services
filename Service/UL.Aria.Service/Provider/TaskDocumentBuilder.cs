using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using Task = UL.Aria.Service.Domain.Entity.Task;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Builds download documents for tasks.
    /// </summary>
    public class TaskDocumentBuilder : ITaskDocumentBuilder
    {
        /// <summary>
        /// Builds a documetn for the specified tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        public Stream Build(IEnumerable<Domain.Entity.Task> tasks)
        {
            var workbook = new Workbook();
            workbook.Worksheets.Clear();
            Build(workbook, tasks);

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

        /// <summary>
        /// Builds the specified tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        public Stream Build(IEnumerable<TaskProjectMapping> tasks)
		{
			var workbook = new Workbook();
			workbook.Worksheets.Clear();
			Build(workbook, tasks);

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
            var styleFlag = new StyleFlag { FontName = true, FontSize = true };
            
            sheet.Cells.ApplyStyle(style, styleFlag);
            var range = sheet.Cells.CreateRange(0, 0, 1, lastColumn);

            range.ApplyStyle(headerStyle, headerStyleFlag);
            
            sheet.AutoFitColumns();

           
        }

        internal void Build(Workbook workbook, IEnumerable<Domain.Entity.TaskProjectMapping> taskProjectMappings)
        {
            var worksheet = workbook.Worksheets.Add(ExcelTemplateKeys.Tasks);

            var lastCell = worksheet.Cells[0, 0].AddTableHeader("Project Name", true);
                AddBasicHeaders(worksheet, false);
            foreach (var taskProjectMapping in taskProjectMappings)
            {
                lastCell = AddTaskProjectMapping(taskProjectMapping, lastCell, 0);
            }
            FinalizeStyle(workbook, 15);
        }

        internal void Build(Workbook workbook, IEnumerable<Domain.Entity.Task> tasks)
        {
            var worksheet = workbook.Worksheets.Add(ExcelTemplateKeys.Tasks);
            var lastCell = AddBasicHeaders(worksheet);
            foreach (var task in tasks)
            {
                lastCell = AddTask(task, lastCell, 0);
            }
            FinalizeStyle(workbook,14);
        }

        private static Cell AddBasicHeaders(Worksheet worksheet, bool isFirst = true)
        {
            var lastCell = worksheet.Cells[0, 0].AddTableHeader("No", isFirst)
                .AddTableHeader("Task Name")
                .AddTableHeader("Progress Flag")
                .AddTableHeader("Est. Start Day No.")
                .AddTableHeader("Est. Duration (hrs)")
                .AddTableHeader("Task Phase")
                .AddTableHeader("Start")
                .AddTableHeader("% Com")
                .AddTableHeader("End")
                .AddTableHeader("Act. Duration (hrs)")
                .AddTableHeader("Pred.")
                .AddTableHeader("Comments")
                .AddTableHeader("Attachments")
                .AddTableHeader("Cl. Bar Hrs")
                ;
            return lastCell;
        }

        private Cell AddTaskProjectMapping(TaskProjectMapping taskProjectMapping, Cell lastCell, int indent)
        {
            var cell = lastCell.Worksheet.Cells[lastCell.Row + 1, 0];
            cell = cell.AddTableValue(taskProjectMapping.ProjectName, true);
            cell = AddTaskColumns(taskProjectMapping.Task, indent, false, cell);
            foreach (var subTask in taskProjectMapping.Task.SubTasks)
            {
                var subTaskProjectMapping = new TaskProjectMapping {ProjectId = taskProjectMapping.ProjectId, ProjectName = taskProjectMapping.ProjectName, Task = subTask};

                cell = AddTaskProjectMapping(subTaskProjectMapping, cell, indent + 1);
            }
            return cell;
        }

        private Cell AddTask(Task task, Cell lastCell, int indent)
        {
            var cell = lastCell.Worksheet.Cells[lastCell.Row + 1, 0];
            cell = AddTaskColumns(task, indent, true, cell);
            foreach (var subTask in task.SubTasks)
            {
                cell = AddTask(subTask, cell, indent + 1);
            }


            return cell;
        }

        private static Cell AddTaskColumns(Task task, int indent, bool isFirst, Cell cell)
        {
            var title = (task.Title + string.Empty);
            return cell.AddTableValue(task.TaskNumber.ToString(), isFirst)
                .AddTableValue(title.PadLeft(title.Length + indent *2, ' '))
                .AddTableValue(task.Progress.GetDisplayName().SpaceIt())
                .AddTableValue(task.EstimatedStartDayNumber + string.Empty)
                .AddTableValue(task.EstimatedDuration + string.Empty)
                .AddTableValue(task.Status.GetDisplayName().SpaceIt())
                .AddTableValue(task.StartDate.HasValue? task.StartDate.Value.ToShortDateString(): string.Empty)
                .AddTableValue(task.PercentComplete.ToString())
                .AddTableValue(task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : string.Empty)
                .AddTableValue(task.ActualDuration + string.Empty)
                .AddTableValue(string.Join(",", task.Predecessors.Select(x => x.TaskNumber).OrderBy(y => y)))
                .AddTableValue(task.Comments.Any() ? "x" : string.Empty)
                .AddTableValue(string.Empty)
                .AddTableValue(task.ClientBarrierHours + string.Empty);
        }
    }
}
