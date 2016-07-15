namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class EntityTaskHistory. This class cannot be inherited.
	/// </summary>
	public sealed class EntityTaskHistory : EntityHistory
	{
		/// <summary>
		/// Gets or sets the task.
		/// </summary>
		/// <value>The task.</value>
		public Task Task { get; set; }
	}
}