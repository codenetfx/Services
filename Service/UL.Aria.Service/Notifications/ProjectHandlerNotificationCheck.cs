using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ProjectHandlerNotificationCheck : IProjectNotificationCheck
	{
		/// <summary>
		/// Checks the specified project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="originalProject">The original project.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		public bool Check(Domain.Entity.Project project, Domain.Entity.Project originalProject, List<Contracts.Dto.NotificationTypeDto> notifications)
		{
			if (!string.Equals( project.ProjectHandler , originalProject.ProjectHandler, StringComparison.OrdinalIgnoreCase))
			{
				notifications.Add(NotificationTypeDto.ProjectHandlerChange);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>
		/// The ordinal.
		/// </value>
		public int Ordinal
		{
			get { return 200; }
		}
	}
}
