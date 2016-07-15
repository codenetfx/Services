using System.Security.Cryptography;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class CryptographyProvider. This class cannot be inherited.
	/// </summary>
	public sealed class CryptographyProvider : ICryptographyProvider
	{
		private readonly ICryptographyProviderConfigurationSource _cryptographyProviderConfigurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptographyProvider"/> class.
		/// </summary>
		/// <param name="cryptographyProviderConfigurationSource">The cryptography provider configuration source.</param>
		public CryptographyProvider(ICryptographyProviderConfigurationSource cryptographyProviderConfigurationSource)
		{
			_cryptographyProviderConfigurationSource = cryptographyProviderConfigurationSource;
		}

		/// <summary>
		/// Creates the encryptor.
		/// </summary>
		/// <returns>ICryptoTransform.</returns>
		public ICryptoTransform CreateEncryptor()
		{
			var crypto = new RijndaelManaged();
			return crypto.CreateEncryptor(_cryptographyProviderConfigurationSource.Key, _cryptographyProviderConfigurationSource.IV);
		}

		/// <summary>
		/// Creates the decryptor.
		/// </summary>
		/// <returns>ICryptoTransform.</returns>
		public ICryptoTransform CreateDecryptor()
		{
			var crypto = new RijndaelManaged();
			return crypto.CreateDecryptor(_cryptographyProviderConfigurationSource.Key, _cryptographyProviderConfigurationSource.IV);
		}
	}
}