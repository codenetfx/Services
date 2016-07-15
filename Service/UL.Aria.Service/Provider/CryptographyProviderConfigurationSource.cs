using System;
using System.Configuration;
using System.Linq;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class CryptographyProviderConfigurationSource.
	/// </summary>
	public class CryptographyProviderConfigurationSource : ICryptographyProviderConfigurationSource
	{
		// ReSharper disable once InconsistentNaming
		private static readonly byte[] _key;
		// ReSharper disable once InconsistentNaming
		private static readonly byte[] _iv;

		static CryptographyProviderConfigurationSource()
		{
			string key =
				ConfigurationManager.AppSettings.GetValue<string>(
					"UL.Aria.Service.CryptographyProvider.Key", null);
			_key = key.Split(',').Select(s => Convert.ToByte(s.Trim(), 16)).ToArray();
			string iv =
				ConfigurationManager.AppSettings.GetValue<string>(
					"UL.Aria.Service.CryptographyProvider.IV", null);
			_iv = iv.Split(',').Select(s => Convert.ToByte(s.Trim(), 16)).ToArray();
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public byte[] Key
		{
			get { return _key; }
		}

		/// <summary>
		/// Gets the iv.
		/// </summary>
		/// <value>The iv.</value>
		public byte[] IV
		{
			get { return _iv; }
		}
	}
}