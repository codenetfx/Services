using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public  interface IProjectNotificationCheckManager
	{
		/// <summary>
		/// Gets the order notifications.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="originalProject">The original project.</param>
		/// <returns></returns>
		List<NotificationTypeDto> GetProjectNotifications(Project project, Project originalProject);
	}
}
