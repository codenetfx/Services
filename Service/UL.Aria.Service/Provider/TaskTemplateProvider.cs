using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provider implementation for Task Templates.
    /// </summary>
    public class TaskTemplateProvider : ITaskTemplateProvider
    {                                   
        private readonly ITaskTemplateRepository _taskTemplateRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTemplateProvider"/> class.
        /// </summary>
        /// <param name="taskTemplateRepository">The task template repository.</param>
        public TaskTemplateProvider(ITaskTemplateRepository taskTemplateRepository)
        {
            _taskTemplateRepository = taskTemplateRepository;
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TaskTemplate FetchById(Guid id)
        {
            return _taskTemplateRepository.FindById(id);
        }

        /// <summary>
        /// Fetches the task template by project template.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="flatten">if set to <c>true</c> [flatten].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<TaskTemplate> FetchTaskTemplateByProjectTemplate(Guid projectId, bool flatten = false)
        {
            var results = _taskTemplateRepository.FetchTaskTemplateByProjectTemplate(projectId);
            return (!flatten) ? GetTree(results, null) : results;         
        }

        /// <summary>
        /// Deletes the specified task template identifier.
        /// </summary>
        /// <param name="taskTemplateId">The task template identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid taskTemplateId)
        {
            _taskTemplateRepository.Remove(taskTemplateId);
        }

        /// <summary>
        /// Creates the specified task template.
        /// </summary>
        /// <param name="taskTemplate">The task template.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TaskTemplate Create(TaskTemplate taskTemplate)
        {
            _taskTemplateRepository.Add(taskTemplate);
            return taskTemplate;
        }

        /// <summary>
        /// Updates the specified task template.
        /// </summary>
        /// <param name="taskTemplate">The task template.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(TaskTemplate taskTemplate)
        {
            _taskTemplateRepository.Update(taskTemplate);
        }

        /// <summary>
        /// Updates the templates in bulk.
        /// </summary>
        /// <param name="projectTemplateId"></param>
        /// <param name="taskTemplates">The task templates.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateBulk(Guid projectTemplateId, IEnumerable<TaskTemplate> taskTemplates)
        {
            var flatList = Flatten(taskTemplates).ToList();
            flatList.ForEach(x => x.ProjectTemplateId = projectTemplateId);
            _taskTemplateRepository.UpdateBulk(projectTemplateId,flatList);
        }

        private IEnumerable<TaskTemplate> Flatten(IEnumerable<TaskTemplate> taskTemplates)
        {
            return taskTemplates.SelectMany(c => Flatten(c.SubTasks)).Concat(taskTemplates);
        }
        
        private IEnumerable<TaskTemplate> GetTree(IEnumerable<TaskTemplate> list, int? parent)
        {
            var children = list.Where(x => x.ParentTaskNumber == parent).ToList();
            children.ForEach(x => {
                x.SubTasks = GetTree(list, x.TaskNumber).ToList();
            });
            return children;
        }
    }
}
