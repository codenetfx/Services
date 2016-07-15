using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Aspose.Cells;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements building download documens for several projects at once
    /// </summary>
    public class MultiProjectDocumentBuilder : IMultiProjectDocumentBuilder
    {
        /// <summary>
        /// Builds the specified project.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns></returns>
        public Stream Build(IEnumerable<ProjectDetail> projects)
        {
            var workbook = BuildInternal(projects);
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

        internal Workbook BuildInternal(IEnumerable<ProjectDetail> projects)
        {
            var workbook = new Workbook();
            workbook.Worksheets.Clear();
            var projectsSheet = AddBlankProjectsSheet(workbook);
            var tasksSheet = AddBlankTasksSheet(workbook);
            foreach (var projectDetail in projects)
            {
                AddProject(projectsSheet, tasksSheet, projectDetail);
            }
            FinalizeStyle(workbook);
            return workbook;
        }

        internal Worksheet AddBlankProjectsSheet(Workbook workbook)
        {
            var worksheet = workbook.Worksheets.Add(ProjectTemplateKeys.ProjectsSheetName);

            var lastFilledCell = worksheet.Cells[0, 0];
            lastFilledCell
                .AddTableHeader(ProjectTemplateKeys.OrderIDLabel, true)
                .AddTableHeader(ProjectTemplateKeys.LineItemIDsLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectIDLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectNameLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectStatusLabel)
                .AddTableHeader(ProjectTemplateKeys.DescriptionLabel)
                .AddTableHeader(ProjectTemplateKeys.StartDateLabel)
                .AddTableHeader(ProjectTemplateKeys.EndDateLabel)
                .AddTableHeader(ProjectTemplateKeys.NumberOfSamplesLabel)
                .AddTableHeader(ProjectTemplateKeys.CompanyLabel)
                .AddTableHeader(ProjectTemplateKeys.SampleReferenceNumbersLabel)
                .AddTableHeader(ProjectTemplateKeys.CcnLabel)
                .AddTableHeader(ProjectTemplateKeys.FileNumberLabel)
                .AddTableHeader(ProjectTemplateKeys.StatusNotesLabel)
                .AddTableHeader(ProjectTemplateKeys.EstEngineeringEffortHoursLabel)
                .AddTableHeader(ProjectTemplateKeys.EstLabEffortHoursLabel)
                .AddTableHeader(ProjectTemplateKeys.EstReviewerEffortHoursLabel)
                .AddTableHeader(ProjectTemplateKeys.EstimatedTatLabel)
                .AddTableHeader(ProjectTemplateKeys.ScopeLabel)
                .AddTableHeader(ProjectTemplateKeys.AssumptionsLabel)
                .AddTableHeader(ProjectTemplateKeys.EngineeringOfficeLimitationsLabel)
                .AddTableHeader(ProjectTemplateKeys.LaboratoryLimitationsLabel)
                .AddTableHeader(ProjectTemplateKeys.ComplexityLabel)
                .AddTableHeader(ProjectTemplateKeys.StandardsLabel)
                .AddTableHeader(ProjectTemplateKeys.InventoryItemCatalogueNosLabel)
                .AddTableHeader(ProjectTemplateKeys.InventoryItemNosDescriptionLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectTypeLabel)
                .AddTableHeader(ProjectTemplateKeys.QuoteNoLabel)
                .AddTableHeader(ProjectTemplateKeys.PriceLabel)
                .AddTableHeader(ProjectTemplateKeys.ExpeditedLabel)
                .AddTableHeader(ProjectTemplateKeys.AdditionalCriteriaLabel)
                .AddTableHeader(ProjectTemplateKeys.IndustryLabel)
                .AddTableHeader(ProjectTemplateKeys.IndustrySubGroupLabel)
                .AddTableHeader(ProjectTemplateKeys.IndustryCategoryLabel)
                .AddTableHeader(ProjectTemplateKeys.LocationLabel)
                .AddTableHeader(ProjectTemplateKeys.ProductGroupLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectTaskTemplateTypeLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectHandlerLabel)
                .AddTableHeader(ProjectTemplateKeys.CustomerPOLabel)
                .AddTableHeader(ProjectTemplateKeys.OrderTypeLabel)
                .AddTableHeader(ProjectTemplateKeys.ServiceDescriptionLabel)
                .AddTableHeader(ProjectTemplateKeys.FileNoLabel)
                .AddTableHeader(ProjectTemplateKeys.CustomerRequestedDateLabel)
                .AddTableHeader(ProjectTemplateKeys.DateBookedLabel)
                .AddTableHeader(ProjectTemplateKeys.DateOrderedLabel)
                .AddTableHeader(ProjectTemplateKeys.LastUpdateDateLabel)
                .AddTableHeader(ProjectTemplateKeys.OrderStartDateLabel)
                .AddTableHeader(ProjectTemplateKeys.OracleProjectIDLabel)
                .AddTableHeader(ProjectTemplateKeys.OracleProjectNameLabel)
                .AddTableHeader(ProjectTemplateKeys.OracleProjectNumberLabel)
                .AddTableHeader(ProjectTemplateKeys.AssociatedProductIDsLabel)
                ;

            return worksheet;
        }

        internal Worksheet AddBlankTasksSheet(Workbook workbook)
        {
            var worksheet = workbook.Worksheets.Add(ProjectTemplateKeys.TasksSheetName);

            var lastFilledCell = worksheet.Cells[0, 0];
            lastFilledCell
                .AddTableHeader(ProjectTemplateKeys.OrderIDLabel, true)
                .AddTableHeader(ProjectTemplateKeys.ProjectIDLabel)
                .AddTableHeader(ProjectTemplateKeys.ProjectNameLabel)
                .AddTableHeader(ProjectTemplateKeys.TaskIDLabel)
                .AddTableHeader(ProjectTemplateKeys.TaskNameLabel)
                .AddTableHeader(ProjectTemplateKeys.ParentIdLabel)
                .AddTableHeader(ProjectTemplateKeys.ChildIdsLabel)
                .AddTableHeader(ProjectTemplateKeys.PredecessorTasksLabel)
                .AddTableHeader(ProjectTemplateKeys.PhaseLabel)
                .AddTableHeader(ProjectTemplateKeys.ProgressLabel)
                .AddTableHeader(ProjectTemplateKeys.StartDateLabel)
                .AddTableHeader(ProjectTemplateKeys.DueDateLabel)
                .AddTableHeader(ProjectTemplateKeys.EstimatedDurationHoursLabel)
                .AddTableHeader(ProjectTemplateKeys.ActualDurationHoursLabel)
                .AddTableHeader(ProjectTemplateKeys.ClientBarrierHoursLabel)
                .AddTableHeader(ProjectTemplateKeys.PercentCompleteLabel)
                .AddTableHeader(ProjectTemplateKeys.TaskOwnerLabel)
                .AddTableHeader(ProjectTemplateKeys.CommentsLabel)
                ;

            return worksheet;
        }

        internal void AddProject(Worksheet projectWorksheet, Worksheet tasksWorksheet, ProjectDetail project)
        {
            var currentRow = projectWorksheet.FindLastFilledRow() + 1;
            projectWorksheet.Cells[currentRow, 0]
                .AddTableValue(project.Project.OrderNumber, true)
                .AddTableValue(string.Join(", ", project.Project.ServiceLines.Select(x => x.LineNumber).ToList()))
                .AddTableValue(project.Project.Id.ToString())
                .AddTableValue(project.Project.Name)
                .AddTableValue(project.Project.ProjectStatus.ToString())
                .AddTableValue(project.Project.Description)
                .AddTableDateValueNullAsEmpty(project.Project.StartDate)
                .AddTableDateValueNullAsEmpty(project.Project.EndDate)
                .AddTableIntValueNullAsEmpty(project.Project.NumberOfSamples)
                .AddTableValue(project.Project.CompanyId.ToString())
                .AddTableValue(project.Project.SampleReferenceNumbers)
                .AddTableValue(project.Project.CCN)
                .AddTableValue(project.Project.FileNo)
                .AddTableValue(project.Project.StatusNotes)
                .AddTableDecimalValueNullAsEmpty(project.Project.EstimateEngineeringEffort)
                .AddTableDecimalValueNullAsEmpty(project.Project.EstimatedLabEffort)
                .AddTableDecimalValueNullAsEmpty(project.Project.EstimatedReviewerEffort)
                .AddTableDateValueNullAsEmpty(project.Project.EstimatedTATDate)
                .AddTableValue(project.Project.Scope)
                .AddTableValue(project.Project.Assumptions)
                .AddTableValue(project.Project.EngineeringOfficeLimitations)
                .AddTableValue(project.Project.LaboratoryLimitations)
                .AddTableValue(project.Project.Complexity)
                .AddTableValue(project.Project.Standards)
                .AddTableValue(project.Project.InventoryItemCatalogNumbers)
                .AddTableValue(project.Project.InventoryItemNumbersDescriptions)
                .AddTableValue(project.Project.ProjectType)
                .AddTableValue(project.Project.QuoteNo)
                .AddTableValue(project.Project.Price)
                .AddTableValue(project.Project.Expedited.ToYesNo())
                .AddTableValue(project.Project.AdditionalCriteria)
                .AddTableValue(project.Project.Industry)
                .AddTableValue(project.Project.IndustrySubcategory)
                .AddTableValue(project.Project.IndustryCategory)
                .AddTableValue(project.Project.Location)
                .AddTableValue(project.Project.ProductGroup)
                .AddTableValue(project.Project.ProjectTemplateName)
                .AddTableValue(project.Project.ProjectHandler)
                .AddTableValue(project.Project.CustomerPo)
                .AddTableValue(project.Project.OrderType)
                .AddTableValue(project.Project.ServiceDescription)
                .AddTableValue(project.Project.FileNo)
                .AddTableDateValueNullAsEmpty(project.Project.CustomerRequestedDate)
                .AddTableDateValueNullAsEmpty(project.Project.DateBooked)
                .AddTableDateValueNullAsEmpty(project.Project.DateOrdered)
                .AddTableDateValueNullAsEmpty(project.Project.LastUpdateDate)
                .AddTableDateValueNullAsEmpty(project.Project.StartDate)
                .AddTableValue(project.Project.ExternalProjectId)
                .AddTableValue(project.Project.ProjectName)
                .AddTableValue(project.Project.ProjectNumber)
                .AddTableValue(string.Join(", ", project.ProductIds))
                ;
            foreach (var task in project.Tasks)
            {
                AddTask(tasksWorksheet, project, task);
            }
        }

        internal void AddTask(Worksheet taskworkSheet, ProjectDetail project, Task task)
        {
            Task parentTask;
            project.ParentTasks.TryGetValue(task.ParentId, out parentTask);
            var currentRow = taskworkSheet.FindLastFilledRow() + 1;
            var lastCell = taskworkSheet.Cells[currentRow, 0]
                .AddTableValue(project.Project.OrderNumber, true)
// ReSharper disable once PossibleInvalidOperationException
                .AddTableValue(project.Project.Id.Value.ToString())
                .AddTableValue(project.Project.Name)
                .AddTableValue(task.TaskNumber)
                .AddTableValue(task.Title)
                .AddTableValue(null != parentTask ? parentTask.TaskNumber.ToString(CultureInfo.InvariantCulture) : string.Empty)
                .AddTableValue(task.SubTasks == null
                                   ? string.Empty
                                   : string.Join(", ", task.SubTasks.Select(x => x.TaskNumber)))
                .AddTableValue(task.Predecessors == null
                                   ? string.Empty
                                   : string.Join(", ", task.Predecessors.Select(x => x.TaskNumber)))
                .AddTableValue(task.Status.GetDisplayName().SpaceIt())
				.AddTableValue(task.Progress.GetDisplayName().SpaceIt())
                .AddTableDateValueNullAsEmpty(task.StartDate)
                .AddTableDateValueNullAsEmpty(task.DueDate)
                .AddTableValue(task.EstimatedDuration)
                .AddTableValue(task.ActualDuration)
                .AddTableValue(task.ClientBarrierHours)
                .AddTableValue(task.PercentComplete)
                .AddTableValue(task.TaskOwner)
                .AddTableValue(task.Comments == null
                                   ? string.Empty
                                   : string.Join(Environment.NewLine, task.Comments.Select(x => x.Comment)))
                ;
            Style style = lastCell.GetStyle();
            style.IsTextWrapped = true;
            
            lastCell.SetStyle(style);
        }

        private void FinalizeStyle(Workbook workbook)
        {
            for (int i = 0; i < 2; ++i)
            {
                var sheet = workbook.Worksheets[i];
                var lastCell = sheet.Cells.LastCell;
                var lastColumn = lastCell.Column;
                
                var headerStyle = new Style();
                headerStyle.Font.Name = "Calibri";
                headerStyle.Font.Size = 11;
                headerStyle.Font.IsBold = true;
                headerStyle.Font.Underline = FontUnderlineType.Single;
                var headerStyleFlag = new StyleFlag
                    {
                        FontName = true,
                        FontSize = true,
                        FontColor = true,
                        CellShading = true,
                        FontUnderline = true,
                        FontBold = true
                    };

                var style = new Style();
                style.Font.Name = "Calibri";
                style.Font.Size = 10;
                style.VerticalAlignment = TextAlignmentType.Top;
                var styleFlag = new StyleFlag {FontName = true, FontSize = true, VerticalAlignment = true};
                
                sheet.Cells.ApplyStyle(style, styleFlag);

                var range = sheet.Cells.CreateRange(0, 0, 1, lastColumn + 1);

                range.ApplyStyle(headerStyle, headerStyleFlag);

                sheet.AutoFitColumns();
            }
        }
    }
}