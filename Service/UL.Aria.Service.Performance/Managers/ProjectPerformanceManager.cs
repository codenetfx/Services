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
	/// Provides an implementation of the PerformanceManagerBase(IPerformanceManager) for the Project Entity to measure project performance.
	/// </summary>
	[Entity(EntityTypeEnumDto.Project)]
	public class ProjectPerformanceManager : PerformanceManagerBase
	{
		private const EntityTypeEnumDto EntityType = EntityTypeEnumDto.Project;
		private readonly IProjectManager _projectManager;
		private readonly IIncomingOrderManager _incomingOrderManager;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectPerformanceManager"/> class.
		/// </summary>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="incomingOrderManager">The incoming order manager.</param>
		/// <param name="fileLogger">The file logger.</param>
		/// <param name="config">The configuration.</param>
		public ProjectPerformanceManager(IProjectManager projectManager, IIncomingOrderManager incomingOrderManager,
			IFileLogger fileLogger, IPerformanceConfigurationSource config)
			: base(fileLogger, config)
		{
			_projectManager = projectManager;
			_incomingOrderManager = incomingOrderManager;
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
			var lookups = new List<Lookup>(); //_projectProvider.FetchProjectLookups();

			for (var i = 0; i < ConfigSource.Iterations; i++)
			{
				var projectCreationId = Guid.NewGuid();
				var project = new ProjectCreationRequest
				{
					CompanyId = Guid.Parse("dc048390-fe92-e211-89fd-54d9dfe94c0d"),
					IsEmailRequested = false,
					IsReadyToCreate = false,
					Name = "p" + projectCreationId,
					ProjectHandler = "portalservices@ul.com",
					ProjectTemplateId = Guid.Parse("221ed115-a594-49c9-ab44-56786ab86cee"),
					StartDate = DateTime.UtcNow
				};

				_projects.Add(projectCreationId, project);

				var lookup = new Lookup {Id = projectCreationId};
				lookups.Add(lookup);
			}

			return lookups;
			//if (!FileLogger.IsExistingFile)
			//	return lookups;

			//var logEntries = FileLogger.GetLog(GetFileLogMappingFunction());

			//if (logEntries == null || logEntries.Count <= 0)
			//	return lookups;

			//var incompleted = from c in lookups
			//	join l in logEntries on c.Id.Value equals l.Lookup.Id.Value into results
			//	from i in results.DefaultIfEmpty()
			//	where i == null
			//	select c;

			//return incompleted.ToList();
		}

		private readonly Dictionary<Guid, ProjectCreationRequest> _projects = new Dictionary<Guid, ProjectCreationRequest>();

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
					var result = _incomingOrderManager.PublishProjectCreationRequest(_projects[lookup.Id.Value]);
					var project = _projectManager.GetProjectById(result);
					_projectManager.Update(project.Id.Value, project);
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