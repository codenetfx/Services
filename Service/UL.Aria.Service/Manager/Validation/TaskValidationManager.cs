using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	///     Validates <see cref="Task" /> objects.
	/// </summary>
	public class TaskValidationManager : ITaskValidationManager
	{

		private readonly IEnumerable<ITaskValidator> _taskValidators;

		/// <summary>
		///     Initializes a new instance of the <see cref="TaskValidationManager" /> class.
		/// </summary>
		/// <param name="taskValidators">The task valuators.</param>
		public TaskValidationManager(IEnumerable<ITaskValidator> taskValidators)
		{
			_taskValidators = taskValidators;
		}


		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <returns>IList&lt;TaskValidationEnumDto&gt;.</returns>
		public IList<TaskValidationEnumDto> Validate(TaskValidationContext taskValidationContext)
		{
			var errors = new List<TaskValidationEnumDto>();

			foreach (var validator in _taskValidators)
			{
				validator.Validate(taskValidationContext, errors);
			}

			return errors;
		}


		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="entitiesToValidate">The entity to validate.</param>
		/// <param name="project">The project.</param>
		/// <returns>Dictionary&lt;Guid, IList&lt;TaskValidationEnumDto&gt;&gt;.</returns>
		[ExcludeFromCodeCoverage]
		public Dictionary<Guid, IList<TaskValidationEnumDto>> Validate(IList<Task> entitiesToValidate, Project project)
		{
			var validationErrors = new Dictionary<Guid, IList<TaskValidationEnumDto>>();

			foreach (var entity in entitiesToValidate)
			{
				var errors = new List<TaskValidationEnumDto>();
				foreach (var validator in _taskValidators)
				{
					var taskValidationContext = new TaskValidationContext
					{
						Project = project,
						Entity = entity,
						OriginalEntity = project.Tasks.FirstOrDefault(x => x.Id == entity.Id)
					};
					validator.Validate(taskValidationContext, errors);
				}
				if (errors.Any())
					validationErrors.Add(entity.Id.Value, errors);

			}
			return validationErrors;
		}

	}
}
