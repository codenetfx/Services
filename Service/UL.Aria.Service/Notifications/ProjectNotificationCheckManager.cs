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
	public class ProjectNotificationCheckManager : IProjectNotificationCheckManager
	{
		private readonly IEnumerable<IProjectNotificationCheck> _notificationChecks;
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectNotificationCheckManager" /> class.
		/// </summary>
		/// <param name="notificationChecks">The notification checks.</param>
		public ProjectNotificationCheckManager(IEnumerable<IProjectNotificationCheck> notificationChecks)
		{
			_notificationChecks = notificationChecks;
		}

		/// <summary>
		/// Gets the order notifications.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="originalProject">The original project.</param>
		/// <returns></returns>
		public List<Contracts.Dto.NotificationTypeDto> GetProjectNotifications(Domain.Entity.Project project, Domain.Entity.Project originalProject)
		{

			var notifications = new List<NotificationTypeDto>();

			foreach (var notificationCheck in _notificationChecks.OrderBy(x => x.Ordinal))
			{
				//break if continue (return value from notification check) is false
				if (!notificationCheck.Check(project, originalProject, notifications))
					break;
			}

			return notifications;

		}
	}
}
