using System;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	///     contract for coordinating project oriented functionality
	/// </summary>
	public interface IProjectManager : IProjectCoreManager
	{
		/// <summary>
		///     Updates the specified project.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="project">The project.</param>
		void Update(Guid id, Project project);
	}
}