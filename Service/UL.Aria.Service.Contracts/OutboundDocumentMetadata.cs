using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts
{
	/// <summary>
	/// Class OutboundDocumentMetadata.
	/// </summary>
	public static class OutboundDocumentMetadata
	{
		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <param name="outboundDocument">The outbound document.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
		/// <value>The metadata.</value>
		public static IDictionary<string, string> Metadata(this OutboundDocumentDto outboundDocument)
		{
			var metadata = new Dictionary<string, string>
			{
				{"IsNew", outboundDocument.IsNew.ToString()},
				{"DocumentId", outboundDocument.DocumentId.ToString()},
				{"PrimarySearchEntityId", outboundDocument.PrimarySearchEntityId.ToString()},
				{"Permission", outboundDocument.Permission.ToString()},
				{"Size", outboundDocument.Size.ToString()},
			};

			if (!string.IsNullOrWhiteSpace(outboundDocument.MessageId))
			{
				metadata["MessageId"] = outboundDocument.MessageId;
			}

			if (!string.IsNullOrWhiteSpace(outboundDocument.ContentType))
			{
				metadata["ContentType"] = outboundDocument.ContentType;
			}

			if (!string.IsNullOrWhiteSpace(outboundDocument.OriginalFileName))
			{
				metadata["OriginalFileName"] = outboundDocument.OriginalFileName;
			}

			if (!string.IsNullOrWhiteSpace(outboundDocument.Title))
			{
				metadata["Title"] = outboundDocument.Title;
			}

			if (!string.IsNullOrWhiteSpace(outboundDocument.Description))
			{
				metadata["Description"] = outboundDocument.Description;
			}

			if (outboundDocument.DocumentTypeId.HasValue)
			{
				metadata["DocumentTypeId"] = outboundDocument.DocumentTypeId.GetValueOrDefault().ToString();
			}

			if (!string.IsNullOrWhiteSpace(outboundDocument.LastModifiedBy))
			{
				metadata["LastModifiedBy"] = outboundDocument.LastModifiedBy;
			}

			return metadata;
		}

		/// <summary>
		/// Assets the metadata.
		/// </summary>
		/// <param name="outboundDocument">The outbound document.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
		public static IDictionary<string, string> AssetMetadata(this OutboundDocumentDto outboundDocument, Guid containerId)
		{
			return new Dictionary<string, string>
			{
				{AssetFieldNames.AriaAssetType, AssetTypeEnumDto.Document.ToString()},
				{AssetFieldNames.AriaContentType, outboundDocument.ContentType},
				{AssetFieldNames.AriaName, outboundDocument.OriginalFileName},
				{AssetFieldNames.AriaTitle, outboundDocument.NormalizedTitle()},
				{AssetFieldNames.AriaProductDescription, outboundDocument.Description},
				{AssetFieldNames.AriaPermission, outboundDocument.Permission.ToString()},
				{AssetFieldNames.AriaDocumentTypeId, outboundDocument.DocumentTypeId.ToString()},
				{AssetFieldNames.AriaSize, outboundDocument.Size.ToString()},
				{AssetFieldNames.AriaLastModifiedBy, outboundDocument.LastModifiedBy},
				{AssetFieldNames.AriaLastModifiedOn, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
				{AssetFieldNames.AriaContainerId, containerId.ToString("N")}
			};
		}

		/// <summary>
		/// Normalizeds the title.
		/// </summary>
		/// <param name="outboundDocument">The outbound document.</param>
		/// <returns>System.String.</returns>
		public static string NormalizedTitle(this OutboundDocumentDto outboundDocument)
		{
			if (string.IsNullOrWhiteSpace(outboundDocument.Title))
				return outboundDocument.OriginalFileName;

			return outboundDocument.Title;
		}

		/// <summary>
		/// Parses the document metadata.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xml">The XML.</param>
		/// <returns>T.</returns>
		public static T ParseDocumentMetadata<T>(string xml) where T : OutboundDocumentDto, new()
		{
			var outboundDocument = new T();
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode root = xmlDocument.DocumentElement;
			// ReSharper disable once PossibleNullReferenceException
			foreach (XmlNode xmlNode in root.ChildNodes)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					var xmlNodeInnerText = xmlNode.InnerText;
					switch (xmlNode.Name.ToLower())
					{
						case "messageid":
							outboundDocument.MessageId = xmlNodeInnerText;
							break;
						case "documentid":
							if (!string.IsNullOrWhiteSpace(xmlNodeInnerText))
							{
								outboundDocument.DocumentId = Guid.Parse(xmlNodeInnerText);
							}
							break;
						case "primarysearchentityid":
							if (!string.IsNullOrWhiteSpace(xmlNodeInnerText))
							{
								outboundDocument.PrimarySearchEntityId = Guid.Parse(xmlNodeInnerText);
							}
							break;
						case "isnew":
							if (!string.IsNullOrWhiteSpace(xmlNodeInnerText))
							{
								outboundDocument.IsNew = Convert.ToBoolean(xmlNodeInnerText);
							}
							break;
						case "contenttype":
							outboundDocument.ContentType = xmlNodeInnerText;
							break;
						case "originalfilename":
							outboundDocument.OriginalFileName = xmlNodeInnerText;
							break;
						case "description":
							outboundDocument.Description = xmlNodeInnerText;
							break;
						case "documenttypeid":
							if (!string.IsNullOrWhiteSpace(xmlNodeInnerText))
							{
								outboundDocument.DocumentTypeId = Guid.Parse(xmlNodeInnerText);
							}
							break;
						case "lastmodifiedby":
							outboundDocument.LastModifiedBy = xmlNodeInnerText;
							break;
						case "permission":
							if (!string.IsNullOrWhiteSpace(xmlNodeInnerText))
							{
								outboundDocument.Permission =
									(DocumentPermissionEnumDto) Enum.Parse(typeof (DocumentPermissionEnumDto), xmlNodeInnerText);
							}
							break;
						case "size":
							if (!string.IsNullOrWhiteSpace(xmlNodeInnerText))
							{
								outboundDocument.Size = Convert.ToInt64(xmlNodeInnerText);
							}
							break;
						case "title":
							outboundDocument.Title = xmlNodeInnerText;
							break;
					}
				}
			}

			return outboundDocument;
		}
	}
}