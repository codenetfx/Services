using System.Configuration;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class InboundMessageProviderConfigurationSource.
	/// </summary>
	public sealed class InboundMessageProviderConfigurationSource : IInboundMessageProviderConfigurationSource
	{
		// ReSharper disable once InconsistentNaming
		private static readonly int _newExpireDays;
		// ReSharper disable once InconsistentNaming
		private static readonly int _failedExpireDays;
		// ReSharper disable once InconsistentNaming
		private static readonly int _orderMessageExpireDays;

		static InboundMessageProviderConfigurationSource()
		{
			_newExpireDays =
				ConfigurationManager.AppSettings.GetValue("InboundMessage.NewExpireDays",
					7);
			_failedExpireDays =
				ConfigurationManager.AppSettings.GetValue("InboundMessage.FailedExpireDays",
					7);
			_orderMessageExpireDays =
				ConfigurationManager.AppSettings.GetValue("InboundMessage.OrderMessageExpireDays",
					7);
		}

		/// <summary>
		/// Gets the new expire days.
		/// </summary>
		/// <value>The new expire days.</value>
		public int NewExpireDays
		{
			get { return _newExpireDays; }
		}

		/// <summary>
		/// Gets the failed expire days.
		/// </summary>
		/// <value>The failed expire days.</value>
		public int FailedExpireDays
		{
			get { return _failedExpireDays; }
		}

		/// <summary>
		/// Gets the order message expire days.
		/// </summary>
		/// <value>The order message expire days.</value>
		public int OrderMessageExpireDays
		{
			get { return _orderMessageExpireDays; }
		}
	}
}