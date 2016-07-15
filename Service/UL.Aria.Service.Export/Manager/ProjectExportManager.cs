using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Export.Common;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    ///     Implements operations for exporting <see cref="Project" /> entities.
    /// </summary>
    public class ProjectExportManager : IProjectExportManager
    {
        private readonly IExportConfiguration _configuration;
        private readonly IProjectManager _projectManager;
        private readonly ICompanyManager _companyManager;
        private readonly IProjectExportDocumentManager _projectExportDocumentManager;
        private readonly IFileStorageManager _fileStorageManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectExportManager" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="projectManager">The projectManager.</param>
        /// <param name="companyManager">The CompanyManager</param>
        /// <param name="projectExportDocumentManager"></param>
        /// <param name="fileStorageManager">The file storage projectManager.</param>
        public ProjectExportManager(IExportConfiguration configuration, IProjectManager projectManager, ICompanyManager companyManager, IProjectExportDocumentManager projectExportDocumentManager,IFileStorageManager fileStorageManager)
        {
            _configuration = configuration;
            _projectManager = projectManager;
            _companyManager = companyManager;
            _projectExportDocumentManager = projectExportDocumentManager;
            _fileStorageManager = fileStorageManager;
            
        }

        /// <summary>
        ///     Exports the projects.
        /// </summary>
        public void ExportProjects()
        {
            var companies = _companyManager.FetchAll().ToDictionary(x => x.Id.Value, y => y);

            var projects = _projectManager.GetAllProjectIds();
            using (var projectStream = new MemoryStream())
            {

                _projectExportDocumentManager.CreateProjectHeader(projectStream);
                _projectExportDocumentManager.ExportProjects(projectStream, projects, companies, _projectManager.GetProjectWithoutTaskRollupsById);
                projectStream.Seek(0, SeekOrigin.Begin);
                var projectExportFile = string.Format(_configuration.ProjectExportFile, DateTime.UtcNow).Replace(":", "_");
                _fileStorageManager.Save(_configuration.ProjectExportStorageDirectory, projectExportFile, projectStream);
            }

            using (var taskStream = new MemoryStream())
                {
                    _projectExportDocumentManager.CreateTaskHeader(taskStream);
                    var maxParallelism = Math.Max(Environment.ProcessorCount -2, 1);
                    var po = new ParallelOptions {MaxDegreeOfParallelism = maxParallelism};
                    Parallel.ForEach(projects, po, projectId =>
                    {
                        var project = _projectManager.GetProjectWithoutTaskRollupsById(projectId);
                        var projectDetail = _projectManager.GetProjectDetail(project);
                        _projectExportDocumentManager.ExportProjectTasks(taskStream, projectDetail);
                    });
                    taskStream.Seek(0, SeekOrigin.Begin);
                    var taskExportFile = string.Format(_configuration.TaskExportFile, DateTime.UtcNow).Replace(":", "_");
                    _fileStorageManager.Save(_configuration.ProjectExportStorageDirectory, taskExportFile, taskStream);
                }
        }
    }
}
