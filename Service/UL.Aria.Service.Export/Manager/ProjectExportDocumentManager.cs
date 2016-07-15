using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using Task = UL.Aria.Service.Domain.Entity.Task;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    ///     Implement operations for exporting <see cref="Project" />s to a file stream.
    /// </summary>
    public class ProjectExportDocumentManager : IProjectExportDocumentManager
    {
        private readonly object _lockObj = new object();

        /// <summary>
        ///     Exports <see cref="Project" />s to the supplied stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="projectIds"></param>
        /// <param name="companies">The companies.</param>
        /// <param name="projectLookupFunc">The project lookup function.</param>
        public void ExportProjects(Stream stream, IEnumerable<Guid> projectIds, Dictionary<Guid, Company> companies,
            Func<Guid, Project> projectLookupFunc)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4068, true))
            {
                int maxParallelism = Math.Max(Environment.ProcessorCount - 2, 1);
                var po = new ParallelOptions {MaxDegreeOfParallelism = maxParallelism};
                Parallel.ForEach(projectIds, po, projectId =>
                {
                    Project project;

                    using (new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions{IsolationLevel = IsolationLevel.ReadUncommitted}))
                    {
                        project = projectLookupFunc(projectId);
                    }
                    string companyName = project.CompanyName;
                    lock (_lockObj)
                    {
                        WriteProject(companies, project, companyName, writer);
                    }
                });
            }
        }

        /// <summary>
        ///     Creates the header in the export document for the project.
        /// </summary>
        /// <param name="stream"></param>
        public void CreateProjectHeader(Stream stream)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4068, true))
            {
                writer.WriteValue(ProjectTemplateKeys.ProjectIDLabel);
                writer.WriteValue(ProjectTemplateKeys.ProjectNameLabel);
                writer.WriteValue(ProjectTemplateKeys.OrderIDLabel);
                writer.WriteValue(ProjectTemplateKeys.LineItemIDsLabel);
                writer.WriteValue(ProjectTemplateKeys.ProjectStatusLabel);
                writer.WriteValue(ProjectTemplateKeys.DescriptionLabel);
                writer.WriteValue(ProjectTemplateKeys.StartDateLabel);
                writer.WriteValue(ProjectTemplateKeys.EndDateLabel);
                writer.WriteValue(ProjectTemplateKeys.CompletionDateLabel);
                writer.WriteValue(ProjectTemplateKeys.NumberOfSamplesLabel);
                writer.WriteValue(ProjectTemplateKeys.CompanyLabel);
                writer.WriteValue(ProjectTemplateKeys.SampleReferenceNumbersLabel);
                writer.WriteValue(ProjectTemplateKeys.CcnLabel);
                writer.WriteValue(ProjectTemplateKeys.FileNumberLabel);
                writer.WriteValue(ProjectTemplateKeys.StatusNotesLabel);
                writer.WriteValue(ProjectTemplateKeys.EstEngineeringEffortHoursLabel);
                writer.WriteValue(ProjectTemplateKeys.EstLabEffortHoursLabel);
                writer.WriteValue(ProjectTemplateKeys.EstReviewerEffortHoursLabel);
                writer.WriteValue(ProjectTemplateKeys.EstimatedTatLabel);
                writer.WriteValue(ProjectTemplateKeys.ScopeLabel);
                writer.WriteValue(ProjectTemplateKeys.AssumptionsLabel);
                writer.WriteValue(ProjectTemplateKeys.EngineeringOfficeLimitationsLabel);
                writer.WriteValue(ProjectTemplateKeys.LaboratoryLimitationsLabel);
                writer.WriteValue(ProjectTemplateKeys.ComplexityLabel);
                writer.WriteValue(ProjectTemplateKeys.StandardsLabel);
                writer.WriteValue(ProjectTemplateKeys.InventoryItemCatalogueNosLabel);
                writer.WriteValue(ProjectTemplateKeys.InventoryItemNosDescriptionLabel);
                writer.WriteValue(ProjectTemplateKeys.ProjectTypeLabel);
                writer.WriteValue(ProjectTemplateKeys.QuoteNoLabel);
                writer.WriteValue(ProjectTemplateKeys.PriceLabel);
                writer.WriteValue(ProjectTemplateKeys.ExpeditedLabel);
                writer.WriteValue(ProjectTemplateKeys.AdditionalCriteriaLabel);
                writer.WriteValue(ProjectTemplateKeys.IndustryLabel);
                writer.WriteValue(ProjectTemplateKeys.IndustrySubGroupLabel);
                writer.WriteValue(ProjectTemplateKeys.IndustryCategoryLabel);
                writer.WriteValue(ProjectTemplateKeys.LocationLabel);
                writer.WriteValue(ProjectTemplateKeys.ProductGroupLabel);
                writer.WriteValue(ProjectTemplateKeys.ProjectTaskTemplateTypeLabel);
                writer.WriteValue(ProjectTemplateKeys.ProjectHandlerLabel);
                writer.WriteValue(ProjectTemplateKeys.CustomerPOLabel);
                writer.WriteValue(ProjectTemplateKeys.OrderTypeLabel);
                writer.WriteValue(ProjectTemplateKeys.ServiceDescriptionLabel);
                writer.WriteValue(ProjectTemplateKeys.FileNoLabel);
                writer.WriteValue(ProjectTemplateKeys.CustomerRequestedDateLabel);
                writer.WriteValue(ProjectTemplateKeys.DateBookedLabel);
                writer.WriteValue(ProjectTemplateKeys.DateOrderedLabel);
                writer.WriteValue(ProjectTemplateKeys.LastUpdateDateLabel);
                writer.WriteValue(ProjectTemplateKeys.OrderStartDateLabel);
                writer.WriteValue(ProjectTemplateKeys.OracleProjectIDLabel);
                writer.WriteValue(ProjectTemplateKeys.OracleProjectNameLabel);
                writer.WriteValue(ProjectTemplateKeys.OracleProjectNumberLabel);
                writer.WriteValue(ProjectTemplateKeys.CreatedLabel);
                writer.WriteValue(ProjectTemplateKeys.ModifiedLabel);
                writer.WriteValue(ProjectTemplateKeys.IndustryLabel);
                writer.WriteValue(ProjectTemplateKeys.ServiceDescriptionLabel);
                writer.WriteValue(ProjectTemplateKeys.LocationLabel);
                writer.WriteLine();
            }
        }

        /// <summary>
        ///     Exports <see cref="Domain.Entity.Task" />s to the supplied stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="projectDetail"></param>
        public void ExportProjectTasks(Stream stream, ProjectDetail projectDetail)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4068, true))
            {
                lock (_lockObj)
                {
                    foreach (Task task in projectDetail.Tasks)
                    {
                        Task parentTask;
                        projectDetail.ParentTasks.TryGetValue(task.ParentId, out parentTask);
                        WriteTask(projectDetail, writer, task, parentTask);
                    }
                }
            }
        }

        /// <summary>
        ///     Creates the headers in the export document for the tasks.
        /// </summary>
        /// <param name="stream"></param>
        public void CreateTaskHeader(Stream stream)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4068, true))
            {
                writer.WriteValue(ProjectTemplateKeys.ProjectIDLabel);
                writer.WriteValue(ProjectTemplateKeys.ProjectNameLabel);
                writer.WriteValue(ProjectTemplateKeys.TaskIDLabel);
                writer.WriteValue(ProjectTemplateKeys.TaskNameLabel);
                writer.WriteValue(ProjectTemplateKeys.ParentIdLabel);
                writer.WriteValue(ProjectTemplateKeys.ChildIdsLabel);
                writer.WriteValue(ProjectTemplateKeys.PredecessorTasksLabel);
                writer.WriteValue(ProjectTemplateKeys.PhaseLabel);
                writer.WriteValue(ProjectTemplateKeys.ProgressLabel);
                writer.WriteValue(ProjectTemplateKeys.StartDateLabel);
                writer.WriteValue(ProjectTemplateKeys.DueDateLabel);
                writer.WriteValue(ProjectTemplateKeys.CompletionDateLabel);
                writer.WriteValue(ProjectTemplateKeys.EstimatedDurationHoursLabel);
                writer.WriteValue(ProjectTemplateKeys.ActualDurationHoursLabel);
                writer.WriteValue(ProjectTemplateKeys.ClientBarrierHoursLabel);
                writer.WriteValue(ProjectTemplateKeys.PercentCompleteLabel);
                writer.WriteValue(ProjectTemplateKeys.TaskOwnerLabel);
                writer.WriteValue(ProjectTemplateKeys.CreatedLabel);
                writer.WriteValue(ProjectTemplateKeys.ModifiedLabel);
                writer.WriteValue(ProjectTemplateKeys.PercentCompleteLabel);
                writer.WriteValue(ProjectTemplateKeys.PhaseLabel);
                writer.WriteValue(ProjectTemplateKeys.IsDeleted);
                writer.WriteLine();
            }
        }

        private static void WriteProject(Dictionary<Guid, Company> companies, Project project, string companyName,
            StreamWriter writer)
        {
            if (project.CompanyId.HasValue && companies.ContainsKey(project.CompanyId.Value))
            {
                companyName = companies[project.CompanyId.Value].Name;
            }
            writer.WriteValue(project.Id);
            writer.WriteValue(project.Name);
            writer.WriteValue(project.OrderNumber);
            writer.WriteValue((string.Join(";", project.ServiceLines.Select(x => x.LineNumber))));
            writer.WriteValue(project.ProjectStatus);
            writer.WriteValue(project.Description);
            writer.WriteValue(project.StartDate);
            writer.WriteValue(project.EndDate);
            writer.WriteValue(project.CompletionDate);
            writer.WriteValue(project.NumberOfSamples);
            writer.WriteValue(companyName);
            writer.WriteValue(project.SampleReferenceNumbers);
            writer.WriteValue(project.CCN);
            writer.WriteValue(project.FileNo);
            writer.WriteValue(project.StatusNotes);
            writer.WriteValue(project.EstimateEngineeringEffort);
            writer.WriteValue(project.EstimatedLabEffort);
            writer.WriteValue(project.EstimatedReviewerEffort);
            writer.WriteValue(project.EstimatedTATDate);
            writer.WriteValue(project.Scope);
            writer.WriteValue(project.Assumptions);
            writer.WriteValue(project.EngineeringOfficeLimitations);
            writer.WriteValue(project.LaboratoryLimitations);
            writer.WriteValue(project.Complexity);
            writer.WriteValue(project.Standards);
            writer.WriteValue(project.InventoryItemCatalogNumbers);
            writer.WriteValue(project.InventoryItemNumbersDescriptions);
            writer.WriteValue(project.ProjectType);
            writer.WriteValue(project.QuoteNo);
            writer.WriteValue(project.Price);
            writer.WriteValue(project.Expedited ? "Yes" : "No");
            writer.WriteValue(project.AdditionalCriteria);
            writer.WriteValue(project.Industry);
            writer.WriteValue(project.IndustrySubcategory);
            writer.WriteValue(project.IndustryCategory);
            writer.WriteValue(project.Location);
            writer.WriteValue(project.ProductGroup);
            writer.WriteValue(project.ProjectTemplateName);
            writer.WriteValue(project.ProjectHandler);
            writer.WriteValue(project.CustomerPo);
            writer.WriteValue(project.OrderType);
            writer.WriteValue(project.ServiceDescription);
            writer.WriteValue(project.FileNo);
            writer.WriteValue(project.CustomerRequestedDate);
            writer.WriteValue(project.DateBooked);
            writer.WriteValue(project.DateOrdered);
            writer.WriteValue(project.LastUpdateDate);
            writer.WriteValue(project.StartDate);
            writer.WriteValue(project.ExternalProjectId);
            writer.WriteValue(project.ProjectName);
            writer.WriteValue(project.ProjectNumber);
            writer.WriteValue(project.CreatedDateTime);
            writer.WriteValue(project.LastUpdateDate);

            IList<IncomingOrderServiceLine> serviceLines = project.ServiceLines ?? new List<IncomingOrderServiceLine>();
            if (serviceLines.Any())
            {
                writer.WriteValue(string.Join(";",
                    serviceLines.Where(x => !String.IsNullOrWhiteSpace(x.IndustryCode))
                        .Select(x => x.IndustryCode)));
                writer.WriteValue(string.Join(";",
                    serviceLines.Where(x => !String.IsNullOrWhiteSpace(x.IndustryCodeLabel))
                        .Select(x => x.IndustryCodeLabel)));
                writer.WriteValue(string.Join(";",
                    serviceLines.Where(x => !String.IsNullOrWhiteSpace(x.ServiceCode))
                        .Select(x => x.ServiceCode)));
                writer.WriteValue(string.Join(";",
                    serviceLines.Where(x => !String.IsNullOrWhiteSpace(x.ServiceCodeLabel))
                        .Select(x => x.ServiceCodeLabel)));
                writer.WriteValue(string.Join(";",
                    serviceLines.Where(x => !String.IsNullOrWhiteSpace(x.LocationName))
                        .Select(x => x.LocationName)));
                writer.WriteValue(string.Join(";",
                    serviceLines.Where(x => !String.IsNullOrWhiteSpace(x.LocationCodeLabel))
                        .Select(x => x.LocationCodeLabel)));
            }
            else //show manual project fields
            {
                writer.WriteValue(project.IndustryCode);
                writer.WriteValue(project.ServiceCode);
                writer.WriteValue(project.Location);
            }

            writer.WriteLine();
        }

        private void WriteTask(ProjectDetail projectDetail, StreamWriter writer, Task task, Task parentTask)
        {
            writer.WriteValue(projectDetail.Project.Id);
            writer.WriteValue(projectDetail.Project.Name);
            writer.WriteValue(task.TaskNumber);
            writer.WriteValue(task.Title);
            writer.WriteValue(null != parentTask
                ? parentTask.TaskNumber.ToString(CultureInfo.InvariantCulture)
                : string.Empty);
            writer.WriteValue(string.Join(";", task.SubTasks.Select(x => x.TaskNumber)));
            writer.WriteValue(string.Join(";", task.Predecessors.Select(x => x.TaskNumber)));
            writer.WriteValue(task.Status);
            writer.WriteValue(task.Progress);
            writer.WriteValue(task.StartDate);
            writer.WriteValue(task.DueDate);
            writer.WriteValue(task.CompletedDate);
            writer.WriteValue(task.EstimatedDuration);
            writer.WriteValue(task.ActualDuration);
            writer.WriteValue(task.ClientBarrierHours);
            writer.WriteValue(task.PercentComplete);
            writer.WriteValue(task.TaskOwner);
            writer.WriteValue(task.Created);
            writer.WriteValue(task.Modified);
            writer.WriteValue(string.Join(";",
                RemoveDuplicateHistory(task.CompletionHistories)
                    .OrderBy(x => x.CreatedDate)
                    .Select(x => string.Format("{0}:{1}", x.Completion, x.CreatedDate))));
            writer.WriteValue(string.Join(";",
                RemoveDuplicateHistory(task.StatusHistories)
                    .OrderBy(x => x.CreatedDate)
                    .Select(x => string.Format("{0}:{1}", x.Status, x.CreatedDate))));
            writer.WriteValue(task.IsDeleted);
            writer.WriteLine();
        }

        internal IList<T> RemoveDuplicateHistory<T>(IEnumerable<T> sourceList) where T : TaskHistoryBase, new()
        {
            var newList = new List<T>();
            var lastItem = new T();
            foreach (T item in sourceList.OrderBy(x => x.CreatedDate))
            {
                if (!item.Value.Equals(lastItem.Value))
                {
                    newList.Add(item);
                }
                lastItem = item;
            }
            return newList;
        }
    }
}