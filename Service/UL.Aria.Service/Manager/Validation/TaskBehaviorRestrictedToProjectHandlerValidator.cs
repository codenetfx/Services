using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Class TaskBehaviorRestrictedToProjectHandlerValidator. This class cannot be inherited.
	/// </summary>
	public sealed class TaskBehaviorRestrictedToProjectHandlerValidator : ITaskValidator
	{
		// ReSharper disable once InconsistentNaming
		private static readonly Dictionary<string, Func<object, object, bool>> _fieldComparisons = new Dictionary
			<string, Func<object, object, bool>>
		{
			{
				"UL.Aria.Service.Domain.Entity.Task.Predecessors",
				(originalValue, newValue) =>
				{
					return ((List<TaskPredecessor>) originalValue).Count == ((List<TaskPredecessor>) newValue).Count &&
					       ((List<TaskPredecessor>) newValue).All(
						       taskPredecessor =>
							       ((List<TaskPredecessor>) originalValue).Any(x => x.TaskNumber == taskPredecessor.TaskNumber));
				}
			}
		};

		// ReSharper disable once InconsistentNaming
		private static readonly Dictionary<Type, Func<object, object, bool>> _typeComparisons = new Dictionary
			<Type, Func<object, object, bool>>
		{
			{
				typeof (string),
				(originalValue, newValue) =>
					string.Equals((string) originalValue, (string) newValue, StringComparison.CurrentCultureIgnoreCase)
			}
		};

		private readonly ILogManager _logManager;

		private readonly IPrincipalResolver _principalResolver;
		private readonly ITaskTypeBehaviorManager _taskTypeBehaviorManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskBehaviorRestrictedToProjectHandlerValidator" /> class.
		/// </summary>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="taskTypeBehaviorManager">The task type behavior manager.</param>
		/// <param name="logManager">The log manager.</param>
		public TaskBehaviorRestrictedToProjectHandlerValidator(IPrincipalResolver principalResolver,
			ITaskTypeBehaviorManager taskTypeBehaviorManager, ILogManager logManager)
		{
			_principalResolver = principalResolver;
			_taskTypeBehaviorManager = taskTypeBehaviorManager;
			_logManager = logManager;
		}

		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
		{
			var originalEntity = taskValidationContext.OriginalEntity;
			if (originalEntity == null)
			{
				return;
			}

			var project = taskValidationContext.Project;
			if (string.Equals(project.ProjectHandler, _principalResolver.Current.Identity.Name,
				StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}

			if (!originalEntity.TaskTypeId.HasValue)
			{
				return;
			}
            
            var entityToValidate = taskValidationContext.Entity;
            if (null == originalEntity.TaskTypeBehaviors || originalEntity.TaskTypeBehaviors.Count() <= 0)
		    {
                originalEntity.TaskTypeBehaviors  = _taskTypeBehaviorManager.FindByTaskTypeId(originalEntity.TaskTypeId.GetValueOrDefault()).ToList();
		        entityToValidate.TaskTypeBehaviors = originalEntity.TaskTypeBehaviors;
		    }
			
			
			var type = originalEntity.GetType();
			var properties = type.GetProperties();

            foreach (var taskBehavior in entityToValidate.TaskTypeBehaviors.Where(x => x.TaskTypeAvailableBehaviorId ==
			                                                      new Guid(
				                                                      TaskTypeAvailableBehaviorFieldDto
					                                                      .RestrictedToProjectHandlerBehavior)))
			{
				var property =
					properties.FirstOrDefault(
						x => string.Equals(x.Name, taskBehavior.FieldName, StringComparison.CurrentCultureIgnoreCase));
				if (property == null)
				{
					_logManager.Log(new LogMessage(MessageIds.TaskBehaviorPropertyNotFound, LogPriority.Critical,
						TraceEventType.Error,
						string.Format("Unable locate task property '{0}'.", taskBehavior.FieldName), LogCategory.User));
				}
                else if (typeof(IEnumerable<int>).IsAssignableFrom(property.PropertyType))
                {
                    var orginalPropertyValue = property.GetValue(originalEntity) as IList<int>;
                    var entityToValidatePropertyValue = property.GetValue(entityToValidate) as IList<int>;

                    if (orginalPropertyValue.Count() != entityToValidatePropertyValue.Count())
                    {
                        errors.Add(TaskValidationEnumDto.TaskBehaviorRestrictedToProjectHandler);
                        break;
                    }
                    else if (orginalPropertyValue.Count() > 0)
                    {
                        if (orginalPropertyValue.Intersect(entityToValidatePropertyValue).Count() != orginalPropertyValue.Count())
                        {
                            errors.Add(TaskValidationEnumDto.TaskBehaviorRestrictedToProjectHandler);
                            break;
                        }
                    }
                }
				else
				{
					if (_fieldComparisons.ContainsKey(type.FullName + "." + property.Name))
					{
						if (
							!_fieldComparisons[type.FullName + "." + property.Name](property.GetValue(originalEntity),
								property.GetValue(entityToValidate)))
						{
							errors.Add(TaskValidationEnumDto.TaskBehaviorRestrictedToProjectHandler);
							break;
						}
						continue;
					}
					if (_typeComparisons.ContainsKey(property.PropertyType))
					{
						if (
							!_typeComparisons[property.PropertyType](property.GetValue(originalEntity), property.GetValue(entityToValidate)))
						{
							errors.Add(TaskValidationEnumDto.TaskBehaviorRestrictedToProjectHandler);
							break;
						}
						continue;
					}
					var comparer = property.GetValue(originalEntity) as IComparable;
					if (comparer != null)
					{
						if (comparer.CompareTo(property.GetValue(entityToValidate)) != 0)
						{
							errors.Add(TaskValidationEnumDto.TaskBehaviorRestrictedToProjectHandler);
							break;
						}
						continue;
					}
					if (property.GetValue(originalEntity) != property.GetValue(entityToValidate))
					{
						errors.Add(TaskValidationEnumDto.TaskBehaviorRestrictedToProjectHandler);
						break;
					}
				}
			}
		}
	}
}