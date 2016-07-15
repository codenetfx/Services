using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates property contains value and within min/max range
    /// </summary>
    public class TaskRequiredStringPropertyValidator : ITaskValidator
    {
        private readonly PropertyInfo _propertyInfo;
        private int? _minLength;
        private int? _maxLength;
        private TaskValidationEnumDto _taskValidationEnum;
        private const BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRequiredStringPropertyValidator" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="taskValidationEnum">The task validation enum.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <exception cref="System.ArgumentException">Property name was not in Task type.</exception>
        public TaskRequiredStringPropertyValidator(string propertyName, TaskValidationEnumDto taskValidationEnum, int? minLength, int? maxLength)
        {
            _propertyInfo = typeof(Task).GetProperty(propertyName, _flags);
            _taskValidationEnum = taskValidationEnum;
            _minLength = minLength ?? 1;
            _maxLength = maxLength ?? int.MaxValue;

            if (_propertyInfo == null)
                throw new ArgumentException("Property name was not in Task type.");

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRequiredStringPropertyValidator" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="taskValidationEnum">The task validation enum.</param>
        /// <param name="minLength">The minimum length.</param>
        public TaskRequiredStringPropertyValidator(string propertyName, TaskValidationEnumDto taskValidationEnum, int? minLength)
            : this(propertyName, taskValidationEnum, minLength, null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRequiredStringPropertyValidator" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="taskValidationEnum">The task validation enum.</param>
        public TaskRequiredStringPropertyValidator(string propertyName, TaskValidationEnumDto taskValidationEnum): this(propertyName, taskValidationEnum, 1)
        {
            
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<Contracts.Dto.TaskValidationEnumDto> errors)
        {
            var value = _propertyInfo.GetValue(taskValidationContext.Entity) as string ?? string.Empty ;
            value = value.Trim();

            if (value.Length < _minLength || value.Length > _maxLength)
            {
                errors.Add(_taskValidationEnum);
            }

            //if ((!string.IsNullOrWhiteSpace(value) && (value.Length < _minLength || value.Length > _maxLength))
            //    || (string.IsNullOrWhiteSpace(value) && _minLength > 0))
            //{
            //    errors.Add(_taskValidationEnum);
            //}
            
        }
    }
}
