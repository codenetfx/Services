﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// 
	/// </summary>
	public class ProjectServiceLineValidator : IProjectValidator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectServiceLineValidator"/> class.
		/// </summary>
		public ProjectServiceLineValidator()
		{
			
		}
		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <param name="originalEntity">The original entity.</param>
		/// <param name="errors">The errors.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Validate(Domain.Entity.Project entityToValidate, Domain.Entity.Project originalEntity, List<Contracts.Dto.ProjectValidationEnumDto> errors)
		{
			if (entityToValidate == null)
			{
				return;
			}

			if (string.IsNullOrEmpty(entityToValidate.ServiceCode) && !entityToValidate.ServiceLines.Any())
			{
				errors.Add(ProjectValidationEnumDto.ServiceLine);
			}
		}
	}
}
