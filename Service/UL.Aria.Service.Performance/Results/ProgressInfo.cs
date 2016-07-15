using System;

namespace UL.Aria.Service.Performance.Results
{
	/// <summary>
	/// Provides a class to send progress information for a long running process.
	/// </summary>
	public class ProgressInfo
	{
		/// <summary>
		/// Gets or sets the completed item identifier of the item that 
		/// completed that trigger this progressInfo to be sent.
		/// </summary>
		/// <value>
		/// The completed item identifier.
		/// </value>
		public Guid CompletedItemId { get; set; }

		/// <summary>
		/// Gets or sets the total items.
		/// </summary>
		/// <value>
		/// The total items.
		/// </value>
		public int TotalItems { get; set; }

		/// <summary>
		/// Gets or sets the processed count.
		/// </summary>
		/// <value>
		/// The processed count.
		/// </value>
		public int ProcessedCount { get; set; }

		/// <summary>
		/// Gets the percentage complete.
		/// </summary>
		/// <value>
		/// The percentage complete.
		/// </value>
		public int PercentageComplete
		{
			get
			{
				if (TotalItems > 0)
					return (int) Math.Floor((ProcessedCount/(double) TotalItems)*100);

				return 100;
			}
		}
	}
}