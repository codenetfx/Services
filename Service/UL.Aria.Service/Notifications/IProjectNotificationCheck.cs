using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells.Drawing;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProjectNotificationCheck : IOrderable
	{

		/// <summary>
		/// Checks the specified project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="originalProject">The original project.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		bool Check(Project project, Project originalProject, List<NotificationTypeDto> notifications );
	}
}
