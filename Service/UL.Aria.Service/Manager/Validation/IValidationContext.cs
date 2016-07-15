using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Interface IValidationContext
	/// </summary>
	public interface IValidationContext
	{
	}

	/// <summary>
	/// Interface IValidationContext
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IValidationContext<T> : IValidationContext where T : ITrackedDomainEntity
	{
		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		/// <value>The entity.</value>
		T Entity { get; set; }

		/// <summary>
		/// Gets or sets the original entity.
		/// </summary>
		/// <value>The original entity.</value>
		T OriginalEntity { get; set; }
	}
}