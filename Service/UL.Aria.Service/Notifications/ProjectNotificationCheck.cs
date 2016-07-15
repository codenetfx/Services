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
	public class ProjectNotificationCheck : IProjectNotificationCheck
	{
		/// <summary>
		/// Checks the specified project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="originalProject">The original project.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Check(Domain.Entity.Project project, Domain.Entity.Project originalProject, List<Contracts.Dto.NotificationTypeDto> notifications)
		{
			return true;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>
		/// The ordinal.
		/// </value>
		public int Ordinal
		{
			get { return 100; }
		}
	}
}
