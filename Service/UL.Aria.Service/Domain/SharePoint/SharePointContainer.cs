using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Domain.SharePoint
{
	/// <summary>
	/// Class Container
	/// </summary>
	public class SharePointContainer
	{
		/// <summary>
		/// The default locale
		/// </summary>
		public const uint DefaultLocale = 1033;

		/// <summary>
		/// Initializes a new instance of the <see cref="Container"/> class.
		/// </summary>
		public SharePointContainer()
		{
			Lists = new List<SharePointContainerList>();
			Groups = new List<SharePointContainerGroup>();
		}

		/// <summary>
		/// Gets or sets the lists.
		/// </summary>
		/// <value>The lists.</value>
		public List<SharePointContainerList> Lists { get; set; }

		/// <summary>
		/// Gets or sets the groups.
		/// </summary>
		/// <value>The groups.</value>
		public List<SharePointContainerGroup> Groups { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		public int Version { get; set; }

		/// <summary>
		/// Gets or sets the type of the container.
		/// </summary>
		/// <value>The type of the container.</value>
		public string ContainerType { get; set; }

		/// <summary>
		/// Gets the locale.
		/// </summary>
		/// <value>The locale.</value>
		public uint Locale
		{
			get { return DefaultLocale; }
		}

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>The path.</value>
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets the partition.
		/// </summary>
		/// <value>The partition.</value>
		public string Partition { get; set; }

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the owner.
		/// </summary>
		/// <value>The owner.</value>
		public string Owner { get; set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return GuidToName(Id); }
		}

		/// <summary>
		/// Gets or sets the meta data.
		/// </summary>
		/// <value>The meta data.</value>
		public Dictionary<string, MetaDataTypeAndValue> MetaData { get; set; }

		/// <summary>
		/// Gets or sets the definition.
		/// </summary>
		/// <value>The definition.</value>
		public string Definition { get; set; }

		/// <summary>
		/// Gets the lists with claims.
		/// </summary>
		/// <returns>System.String.</returns>
		public string GetListsWithClaims()
		{
			const string claimProvider = "ClaimProvider:AriaClaimProvider";
			string xml;
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
					xmlWriter.WriteStartElement("Lists");
					if (Lists != null && Groups != null)
					{
						foreach (var list in Lists)
						{
							xmlWriter.WriteStartElement("List");
							xmlWriter.WriteStartElement("Name");
							xmlWriter.WriteString(list.ListName);
							xmlWriter.WriteEndElement();
							var claims = "";

							if (list.GroupPermissions != null)
							{
								claims =
									list.GroupPermissions.Select(
										gp =>
											Groups.FirstOrDefault(g => string.Compare(g.GroupName, gp.GroupName, StringComparison.OrdinalIgnoreCase) == 0))
										.Where(@group => @group != null && @group.Claims != null)
										.SelectMany(@group => @group.Claims)
										.Aggregate(claims,
											(current, claim) =>
												current + string.Concat(claim.ClaimType, "|", claim.ClaimValue, "|", claimProvider, Environment.NewLine));
							}

							xmlWriter.WriteStartElement("Claims");
							xmlWriter.WriteString(claims);
							xmlWriter.WriteEndElement();

							xmlWriter.WriteEndElement();
						}
					}
					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					xml = stringWriter.ToString();
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

			return xml;
		}

		/// <summary>
		/// GUIDs to name.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>System.String.</returns>
		public static string GuidToName(Guid id)
		{
			return id.ToString("N");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static SharePointContainer Parse(string xml)
		{
			var doc = ParseContainerXml(xml);
			var container = new SharePointContainer();
			container.Definition = xml;
			container.ContainerType = GetAttributeValue(doc.Root, "containerType");
			container.Version = int.Parse(GetAttributeValue(doc.Root, "version"));

			var id = GetAttributeValue(doc.Root, "containerId", false);
			if (!string.IsNullOrEmpty(id))
			{
				container.Id = id.ToGuid();
			}

			container.Groups = GetElement(doc.Root, "groups").Elements("group").Select(g => new SharePointContainerGroup
			{
				GroupName = GetAttributeValue(g, "name"),
				Claims = GetElement(g, "claims").Elements("claim").Select(x => new SharePointContainerClaim
				{
					ClaimType = GetAttributeValue(x, "claimType"),
					ClaimValue = GetAttributeValue(x, "claimValue"),
					ClaimValueType = GetAttributeValue(x, "claimValueType")
				}).ToList()
			}).ToList();

			container.Lists = GetElement(doc.Root, "lists").Elements("list").Select(l => new SharePointContainerList
			{
				ListName = GetAttributeValue(l, "name"),
				ListType =
					(SharePointContainerListType) Enum.Parse(typeof (SharePointContainerListType), GetAttributeValue(l, "type"), true),
				GroupPermissions =
					GetElement(l, "groups", false) != null
						? GetElement(l, "groups").Elements("group").Select(x => new SharePointContainerGroupPermission
						{
							GroupName = GetAttributeValue(x, "name"),
							Permission = GetAttributeValue(x, "permission")
						}).ToList()
						: new List<SharePointContainerGroupPermission>()
			}).ToList();


			return container;
		}

		private static XDocument ParseContainerXml(string xml)
		{
			Guard.IsNotNullOrEmptyTrimmed(xml, "xml");
			try
			{
				var doc = XDocument.Parse(xml);
				if (doc.Root == null || doc.Root.Name != "ariaContainer")
				{
					throw new InvalidOperationException("Xml document does not have a root element 'ariaContainer'");
				}
				return doc;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to parse container xml.", ex);
			}
		}

		private static string GetAttributeValue(XElement element, string name)
		{
			return GetAttributeValue(element, name, true);
		}

		private static string GetAttributeValue(XElement element, string name, bool throwOnNoValue)
		{
			Guard.IsNotNull(element, "element");
			Guard.IsNotNullOrEmptyTrimmed(name, "name");
			var att = element.Attribute(name);
			if (att == null)
			{
				throw new InvalidOperationException(string.Format("Attribute '{0}' does not exist on element '{1}'", name,
					element.Name));
			}
			if (throwOnNoValue && string.IsNullOrWhiteSpace(att.Value))
			{
				throw new InvalidOperationException(string.Format("Attribute '{0}' on element '{1}' does not contain a value", name,
					element.Name));
			}

			return att.Value;
		}

		private static XElement GetElement(XElement element, string name)
		{
			return GetElement(element, name, true);
		}

		private static XElement GetElement(XElement element, string name, bool throwError)
		{
			Guard.IsNotNull(element, "element");
			var child = element.Element(name);
			if (child == null && throwError)
			{
				throw new InvalidOperationException(string.Format("Child element '{0}' of '{1}' was null", name, element.Name));
			}
			return child;
		}
	}
}