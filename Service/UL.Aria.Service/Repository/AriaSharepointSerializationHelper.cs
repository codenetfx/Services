using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// String related extension methods
	/// </summary>
	internal static class AriaSharepointSerializationHelper
	{

        /// <summary>
        /// Serializes the dictionary to SP Xml.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="assetFieldMetadata">The asset field metadata.</param>
        /// <returns>System.String.</returns>
        public static string ToAriaSharepointXml(this IDictionary<string, string> dictionary, IAssetFieldMetadata assetFieldMetadata)
        {
            StringWriter stringWriter = null;
            try
            {

                stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = null;
                try
                {

                    xmlWriter = new XmlTextWriter(stringWriter);
                    var xsn = new XmlSerializerNamespaces();
                    xsn.Add("", "");
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("ariaAssetMetadata");
                    foreach (var name in dictionary.Keys)
                    {
                        xmlWriter.WriteStartElement(name);
                        xmlWriter.WriteAttributeString("TypeName",
													   name == "ariaDocId" ? "System.Int64" : name == "ariaLockedDateTime" ? "System.DateTime?" : "System.String");
                        var assetField = assetFieldMetadata.GetContainerAssetField(AssetTypeEnumDto.Document, name);
                        if (assetField != null)
                        {
							PrimarySearchEntityBase.WriteSpAttributes(xmlWriter, assetField);
                        }
                        xmlWriter.WriteString(dictionary[name]);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    return stringWriter.ToString();
                }
                finally
                {
                    if (xmlWriter != null)
                    {
                        xmlWriter.Dispose();
                        stringWriter = null;
                    }
                }

            }
            finally
            {
                if (stringWriter != null)
                    stringWriter.Dispose();
            }
        }

        /// <summary>
        /// Deserializes the SP Xml to a dictionary.
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <returns></returns>
        public static IDictionary<string, string> ToIDictionaryFromAriaSharepointXml(this string xml)
        {
            var dictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(xml))
            {
                StringReader stringReader = null;
                try
                {
                    stringReader = new StringReader(xml);
                    using (var xmlReader = new XmlTextReader(stringReader))
                    {
                        stringReader = null;

                        // Hack right now for Fizzware builder passing in bad xml
                        try
                        {
                            xmlReader.ReadStartElement();

                            while (!xmlReader.EOF)
                            {

                                switch (xmlReader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        dictionary[xmlReader.Name] = xmlReader.ReadElementString();
                                        break;
                                    default:
                                        xmlReader.Read();
                                        break;
                                }
                            }

                        }
                        catch { }
                    }
                }
                finally
                {
                    if (stringReader != null)
                        stringReader.Dispose();
                }
            }

            return dictionary;
        }
    }
}
