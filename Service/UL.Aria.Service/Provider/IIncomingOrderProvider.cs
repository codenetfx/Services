using System;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Interface IIncomingOrderProvider
	/// </summary>
	public interface IIncomingOrderProvider : IIncomingOrderCoreProvider
	{
		/// <summary>
		/// Publishes the project creation request.
		/// </summary>
		/// <param name="projectCreationRequest">The project creation request.</param>
		Guid PublishProjectCreationRequest(ProjectCreationRequest projectCreationRequest);
	}
}