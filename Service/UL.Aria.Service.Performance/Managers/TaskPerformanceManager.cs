using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Performance.Configuration;
using UL.Aria.Service.Performance.Results;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Performance.Managers
{
	/// <summary>
	/// Provides an implementation of the PerformanceManagerBase(IPerformanceManager) for the Task Entity to measure task performance.
	/// </summary>
	[Entity(EntityTypeEnumDto.Task)]
	public class TaskPerformanceManager : PerformanceManagerBase
	{
		private const EntityTypeEnumDto EntityType = EntityTypeEnumDto.Task;
		private readonly IProjectManager _projectManager;
		private readonly IIncomingOrderManager _incomingOrderManager;
		private readonly ITaskManager _taskManager;


		/// <summary>
		/// Initializes a new instance of the <see cref="TaskPerformanceManager" /> class.
		/// </summary>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="incomingOrderManager">The incoming order manager.</param>
		/// <param name="taskManager">The project provider.</param>
		/// <param name="fileLogger">The file logger.</param>
		/// <param name="config">The configuration.</param>
		public TaskPerformanceManager(IProjectManager projectManager, IIncomingOrderManager incomingOrderManager,
			ITaskManager taskManager, IFileLogger fileLogger,
			IPerformanceConfigurationSource config)
			: base(fileLogger, config)
		{
			_projectManager = projectManager;
			_incomingOrderManager = incomingOrderManager;
			_taskManager = taskManager;
		}

		/// <summary>
		/// Gets the file log mapping function.
		/// </summary>
		/// <returns></returns>
		internal Func<string, TaskExecutionResult> GetFileLogMappingFunction()
		{
			Func<string, TaskExecutionResult> func = stringToMap =>
			{
				TaskExecutionResult result = null;
				var values = stringToMap.Split('\t');
				if (values.Length == 4)
				{
					result = new TaskExecutionResult(new Lookup
					{
						Id = Guid.Parse(values[0]),
						Name = values[1]
					},
						values[2].ParseOrDefault(false),
						values[3]);
				}

				return result;
			};

			return func;
		}

		/// <summary>
		/// When implemented in a derived class it resolves a list of items that were not completed
		/// during a previous execution of the process.
		/// </summary>
		/// <returns></returns>
		protected internal override IEnumerable<Lookup> ResolveIncompleteItems()
		{
			var projectCreationRequest = new ProjectCreationRequest
			{
				CompanyId = Guid.Parse("dc048390-fe92-e211-89fd-54d9dfe94c0d"),
				IsEmailRequested = false,
				IsReadyToCreate = false,
				Name = "p" + Guid.NewGuid(),
				ProjectHandler = "portalservices@ul.com",
				ProjectTemplateId = Guid.Parse("221ed115-a594-49c9-ab44-56786ab86cee"),
				StartDate = DateTime.UtcNow
			};
			var result = _incomingOrderManager.PublishProjectCreationRequest(projectCreationRequest);
			_project = _projectManager.GetProjectById(result);
			_containerId = _project.ContainerId.Value;

			var lookups = new List<Lookup>();

			for (var i = 0; i < ConfigSource.Iterations; i++)
			{
				var taskCreationId = Guid.NewGuid();
				var task = new Task
				{
					ModifiedBy = "portalservices@ul.com",
					Progress =  TaskProgressEnumDto.OnTrack,
					Status =  TaskStatusEnumDto.NotScheduled,
					TaskOwner = "foo@user.com",
					Title =  "t" + taskCreationId
				};
				//TODO:Add parent task after new code checked in
				_tasks.Add(taskCreationId, task);

				var lookup = new Lookup {Id = taskCreationId};
				lookups.Add(lookup);
			}

			return lookups;
		}

		private Guid _containerId;
		private Project _project;
		private readonly Dictionary<Guid, Task> _tasks = new Dictionary<Guid, Task>();

		/// <summary>
		/// When implemented in a derived class it gets the Item Processing function.
		/// </summary>
		/// <param name="lookup">The lookup.</param>
		/// <returns></returns>
		protected internal override Func<TaskExecutionResult> GetItemProcessingFunction(Lookup lookup)
		{
			Func<TaskExecutionResult> func = () =>
			{
				try
				{
					var result = _taskManager.Create(_containerId, _tasks[lookup.Id.Value]);
					var task = _taskManager.FetchById(_containerId, result);
					_taskManager.Update(_containerId, task);
					return new TaskExecutionResult(lookup, true);
				}
				catch (Exception ex)
				{
					return new TaskExecutionResult(lookup, false, ex.Message);
				}
			};

			return func;
		}
	}
}