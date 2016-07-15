using System;
using System.Configuration;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class DocumentContentProviderConfigurationSource. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentContentProviderConfigurationSource : IDocumentContentProviderConfigurationSource
	{
		// ReSharper disable once InconsistentNaming
		private readonly Uri _documentServiceUri;
		private readonly bool _isOutboundDocumentEnabled;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentContentProviderConfigurationSource"/> class.
		/// </summary>
		public DocumentContentProviderConfigurationSource()
		{
			_documentServiceUri =
				new Uri(ConfigurationManager.AppSettings.GetValue("UL.Aria.Service.DocumentContentService.Uri",
					"http://ariasvc:802/DocumentContent/"));
			_isOutboundDocumentEnabled =
				ConfigurationManager.AppSettings.GetValue("UL.Aria.Service.DocumentContentService.IsOutboundDocumentEnabled",
					false);
		}

		/// <summary>
		/// Gets the document service URI.
		/// </summary>
		/// <value>The document service URI.</value>
		public Uri DocumentServiceUri
		{
			get { return _documentServiceUri; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is outbound document enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is outbound document enabled; otherwise, <c>false</c>.</value>
		public bool IsOutboundDocumentEnabled
		{
			get { return _isOutboundDocumentEnabled; }
		}
	}
}