using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Class TaskValidationContext.
	/// </summary>
	public class TaskValidationContext : IValidationContext<Task>
	{
		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		/// <value>The entity.</value>
		public Task Entity { get; set; }

		/// <summary>
		/// Gets or sets the original entity.
		/// </summary>
		/// <value>The original entity.</value>
		public Task OriginalEntity { get; set; }

		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
		public Project Project { get; set; }
	}
}