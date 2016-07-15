using System.Security.Cryptography;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface ICryptographyProvider
	/// </summary>
	public interface ICryptographyProvider
	{
		/// <summary>
		/// Creates the encryptor.
		/// </summary>
		/// <returns>ICryptoTransform.</returns>
		ICryptoTransform CreateEncryptor();

		/// <summary>
		/// Creates the decryptor.
		/// </summary>
		/// <returns>ICryptoTransform.</returns>
		ICryptoTransform CreateDecryptor();
	}
}