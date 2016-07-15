using System;
using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Interface IProjectCoreManager
	/// </summary>
	public interface IProjectCoreManager
	{
		/// <summary>
		///     Gets the project by id.
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns></returns>
		Project GetProjectById(Guid projectId);

		/// <summary>
		///     Gets the project export.
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		Stream GetProjectDownload(Guid projectId);

		/// <summary>
		/// Gets the multiple project download.
		/// </summary>
		/// <param name="projectIds">The project ids.</param>
		/// <returns></returns>
		Stream GetMultipleProjectDownload(IEnumerable<Guid> projectIds);

		/// <summary>
		/// Updates the specified project.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="project">The project.</param>
		/// <param name="sendProjectCompleteEmail">if set to <c>true</c> [send project complete email].</param>
		/// <param name="additionalWorkCreateProjectTemplateTasks">The create project template tasks.</param>
		void Update(Guid id, Project project, bool sendProjectCompleteEmail, Action<Guid?, Guid> additionalWorkCreateProjectTemplateTasks = null);

		/// <summary>
		/// Gets all of the <see cref="Project"/>s. use only for things like exports, never from the UI.
		/// </summary>
		/// <returns>the <see cref="Project"/>s.</returns>
		IEnumerable<Project> GetAllProjects();

		/// <summary>
		/// Gets all of the <see cref="Task"/> objects associated with a given <see cref="Project"/> flattened for that project.
		/// </summary>
		/// <param name="project"></param>
		/// <returns>the <see cref="Task"/>s.</returns>
		ProjectDetail GetProjectDetail(Project project);

		/// <summary>
		/// Validates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="project">The project.</param>
		/// <returns></returns>
		IList<ProjectValidationEnumDto> Validate(string id, Project project);

		/// <summary>
		/// Updates the status from order.
		/// </summary>
		/// <param name="incomingOrder">The incoming order.</param>
		void UpdateStatusFromOrder(IncomingOrder incomingOrder);

		/// <summary>
		/// Fetches the by order number.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		/// <returns></returns>
		IList<Project> FetchByOrderNumber(string orderNumber);

		/// <summary>
		/// Gets all project headerss.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Guid> GetAllProjectIds();

		/// <summary>
		/// Gets the project without task rollups(IsOnTrack will not be set correctly) by identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		Project GetProjectWithoutTaskRollupsById(Guid projectId);
	}
}