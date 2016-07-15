using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	///     Container entity.
	/// </summary>
	[Serializable]
    public class PrimarySearchEntityBase : TrackedDomainEntity
	{
		private static readonly Dictionary<Type, string> _typeConversions = new Dictionary<Type, string>
		{
			{typeof (Int64), typeof (Int64).FullName},
			{typeof (Int32), typeof (Int32).FullName},
			{typeof (Int16), typeof (Int16).FullName},
			{typeof (Decimal), typeof (Decimal).FullName},
			{typeof (Double), typeof (Double).FullName},
			{typeof (Boolean), typeof (Boolean).FullName},
			{typeof (Byte), typeof (Byte).FullName},
			{typeof (DateTime), typeof (DateTime).FullName},
			{typeof (Int64?), typeof (Int64).FullName},
			{typeof (Int32?), typeof (Int32).FullName},
			{typeof (Int16?), typeof (Int16).FullName},
			{typeof (Decimal?), typeof (Decimal).FullName},
			{typeof (Double?), typeof (Double).FullName},
			{typeof (Boolean?), typeof (Boolean).FullName},
			{typeof (Byte?), typeof (Byte).FullName},
			{typeof (DateTime?), typeof (DateTime).FullName}
		};

		/// <summary>
		/// Gets the security group.
		/// </summary>
		/// <value>
		/// The security group.
		/// </value>
		protected virtual string SecurityGroup
		{
			get { return "ReadOnly"; }
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PrimarySearchEntityBase" /> class.
		/// </summary>
		public PrimarySearchEntityBase() : this(null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PrimarySearchEntityBase" /> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public PrimarySearchEntityBase(Guid? id)
			: base(id)
		{
			CreateContainer = true;
		}

		/// <summary>
		///     Gets or sets a value indicating whether [create container].
		/// </summary>
		/// <value>
		///     <c>true</c> if [create container]; otherwise, <c>false</c>.
		/// </value>
		internal bool CreateContainer { get; set; }

		/// <summary>
		///     Gets or sets the db container id.
		/// </summary>
		/// <value>The db container id.</value>
		public Guid? ContainerId { get; set; }

		/// <summary>
		///     Gets or sets the Aria name.  This is the name that can be edited in the Aria portal.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		///     Gets or sets the container type.
		/// </summary>
		/// <value>
		///     The type.
		/// </value>
		public EntityTypeEnumDto Type { get; set; }

		/// <summary>
		///     Gets or sets the company id.
		/// </summary>
		/// <value>
		///     The company id.
		/// </value>
		public Guid? CompanyId { get; set; }

		/// <summary>
		///     Gets the share point metadata.
		/// </summary>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		/// <returns>System.String.</returns>
		public string GetContainerMetadata(IAssetFieldMetadata assetFieldMetadata)
		{
			MemoryStream memoryStream = null;
			string metadata;

			try
			{
				memoryStream = new MemoryStream();
				XmlWriter xmlWriter = null;

				try
				{
					var xmlWriterSettings = new XmlWriterSettings {Encoding = new UTF8Encoding()};
					xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
					var xsn = new XmlSerializerNamespaces();
					xsn.Add("", "");
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("ariaAssetMetadata");

					var assetFields = assetFieldMetadata.GetContainerAssetFieldsForContainer(Type);
					var product = this as Product;
					SerializeParent(xmlWriter, this, assetFieldMetadata, true);


					foreach (var assetFieldKeyValuePair in assetFields)
					{
						if (assetFieldKeyValuePair.Value.IsCharacteristic)
						{
							if (product != null)
							{
								var characteristic =
									product.Characteristics.FirstOrDefault(
										c => c.ProductFamilyCharacteristicId.ToString("N") == assetFieldKeyValuePair.Key);

								if (characteristic != null)
									Product.WriteCharacteristicSpMetadataElement(xmlWriter, characteristic,
										assetFieldMetadata, Type);
							}
						}
						//else
						//{
						//    var assetField = assetFieldKeyValuePair.Value;
						//    var propertyInfo = objectType.GetProperty(assetFieldKeyValuePair.Key);
						//    if (propertyInfo != null)
						//    {
						//        var isAssetFieldSpecified = true;
						//        if (assetField == null)
						//        {
						//            isAssetFieldSpecified = false;
						//            assetField = new AssetFieldMetadata.AssetField(null);
						//        }
						//        if (assetField.Ignore)
						//            continue;

						//        var o = propertyInfo.GetValue(this);

						//        var isAssignableFromEnumerable = o is IEnumerable && !(o is String);

						//        if (isAssignableFromEnumerable)
						//        {
						//            foreach (var item in (IEnumerable)o)
						//                SerializeChild(xmlWriter, string.Empty, item, assetFieldMetadata, true);

						//            continue;
						//        }

						//        var isAssignableFrom = o is DomainEntity;

						//        if (isAssignableFrom)
						//        {
						//            SerializeChild(xmlWriter, string.Empty, o, assetFieldMetadata, true);
						//            continue;
						//        }

						//        if ((!assetField.IncludeInContainer || !isAssetFieldSpecified))
						//            continue;
						//        WriteSpMetadataElement(xmlWriter, propertyInfo, string.Empty,
						//            propertyInfo.GetValue(this), Type,
						//            assetFieldMetadata);

						//    }
						//    //else

						//}
					}

					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					metadata = new UTF8Encoding().GetString(memoryStream.ToArray());
				}
				finally
				{
					if (xmlWriter != null)
					{
						xmlWriter.Dispose();
						memoryStream = null;
					}
				}
			}
			finally
			{
				if (memoryStream != null)
					memoryStream.Dispose();
			}

			return metadata;
		}

		/// <summary>
		///     Serializes this instance.
		/// </summary>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		/// <returns>System.String.</returns>
		public MemoryStream GetAssetMetadata(IAssetFieldMetadata assetFieldMetadata)
		{
			var memoryStream = new MemoryStream();
			var xmlWriterSettings = new XmlWriterSettings {Encoding = new UTF8Encoding()};
			using (var xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
			{
				var xsn = new XmlSerializerNamespaces();
				xsn.Add("", "");
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("ariaAssetMetadata");

				xmlWriter.WriteStartElement(AssetFieldNames.AriaPermission);
				xmlWriter.WriteAttributeString("TypeName", "System.String");
				WriteSpAttributes(xmlWriter, new AssetFieldMetadata.AssetField(AssetFieldNames.AriaPermission));

				xmlWriter.WriteString(SecurityGroup);
				xmlWriter.WriteEndElement();

				SerializeParent(xmlWriter, this, assetFieldMetadata);
				xmlWriter.WriteEndElement();
			}

			memoryStream.Seek(0, SeekOrigin.Begin);
			return memoryStream;
		}

		/// <summary>
		///     Serializes the parent.
		/// </summary>
		/// <param name="xmlWriter">The XML writer.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		/// <param name="isContainerOnly"></param>
		protected virtual void SerializeParent(XmlWriter xmlWriter, object parent, IAssetFieldMetadata assetFieldMetadata,
			bool isContainerOnly = false)
		{
			var parentType = parent.GetType();
			SerializeChildren(xmlWriter, String.Empty, parent, parentType, assetFieldMetadata, isContainerOnly);
		}

		private void SerializeChild(XmlWriter xmlWriter, string objectName, object child,
			IAssetFieldMetadata assetFieldMetadata, bool isContainerOnly = false)
		{
			var childType = child.GetType();
			var childName = objectName + childType.Name + '.';
			SerializeChildren(xmlWriter, childName, child, childType, assetFieldMetadata, isContainerOnly);
		}

		private static readonly Dictionary<Type, PropertyInfo[]> _propertyInfos = new Dictionary<Type, PropertyInfo[]>();

		private static IEnumerable<PropertyInfo> GetPropertyInfo(Type type)
		{
			PropertyInfo[] propertyInfo;

			if (_propertyInfos.ContainsKey(type))
			{
				propertyInfo = _propertyInfos[type];
			}
			else
			{
				propertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
				_propertyInfos.Add(type, propertyInfo);
			}

			return propertyInfo;
		}

		private void SerializeChildren(XmlWriter xmlWriter, string childName, object child, Type childType,
			IAssetFieldMetadata assetFieldMetadata, bool isContainerOnly = false)
		{
			var propertyInfos = GetPropertyInfo(childType);
			foreach (
				var propertyInfo in
					propertyInfos)
			{
				var assetField =
					assetFieldMetadata.GetContainerAssetField(Type, childName + propertyInfo.Name);
				var isAssetFieldSpecified = true;
				if (assetField == null)
				{
					isAssetFieldSpecified = false;
					assetField = new AssetFieldMetadata.AssetField(null);
				}
				if (assetField.Ignore)
					continue;

				var o = propertyInfo.GetValue(child);

				var isAssignableFromEnumerable = o is IEnumerable && !(o is String);

				if (isAssignableFromEnumerable)
				{
					foreach (var item in (IEnumerable) o)
						SerializeChild(xmlWriter, childName, item, assetFieldMetadata, isContainerOnly);

					continue;
				}

				var isAssignableFrom = o is DomainEntity;

				if (isAssignableFrom)
				{
					SerializeChild(xmlWriter, childName, o, assetFieldMetadata, isContainerOnly);
					continue;
				}

				if (isContainerOnly && (!assetField.IncludeInContainer || !isAssetFieldSpecified))
					continue;

				WriteSpMetadataElement(xmlWriter, propertyInfo, childName, o, Type, assetFieldMetadata);
			}
		}

		/// <summary>
		///     Writes the sp metadata element.
		/// </summary>
		/// <param name="xmlWriter">The XML writer.</param>
		/// <param name="propertyInfo">The property info.</param>
		/// <param name="parentsName">Name of the parents.</param>
		/// <param name="o">The o.</param>
		/// <param name="type">The type.</param>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		public static void WriteSpMetadataElement(XmlWriter xmlWriter, PropertyInfo propertyInfo, string parentsName,
			object o, EntityTypeEnumDto type,
			IAssetFieldMetadata assetFieldMetadata)
		{
			var containerType = type.ToString();
			var assetField =
				assetFieldMetadata.GetContainerAssetField(type, parentsName + propertyInfo.Name) ??
				new AssetFieldMetadata.AssetField(null);
			var text = Convert.ToString(o);
			if (assetField.SuppressEmpty && String.IsNullOrWhiteSpace(text))
				return;

			if (assetField.Ignore)
				return;

			if (assetField.IsLowerCase)
				text = text.ToLower();

			if (string.IsNullOrEmpty(assetField.Name))
				assetField.Name = "aria" + containerType + parentsName + propertyInfo.Name;

			xmlWriter.WriteStartElement(assetField.Name);
			xmlWriter.WriteAttributeString("TypeName", GetSpTypeName(propertyInfo.PropertyType));

			WriteSpAttributes(xmlWriter, assetField);


			xmlWriter.WriteString(text);
			xmlWriter.WriteEndElement();
		}

		private static string GetSpTypeName(Type type)
		{
			var typeName = "System.String";

			if (_typeConversions.ContainsKey(type))
			{
				typeName = _typeConversions[type];
			}

			return typeName;
		}

		/// <summary>
		///     Writes the sp attributes.
		/// </summary>
		/// <param name="xmlWriter">The XML writer.</param>
		/// <param name="assetField">The asset field.</param>
		public static void WriteSpAttributes(XmlWriter xmlWriter, AssetFieldMetadata.AssetField assetField)
		{
			if (!string.IsNullOrEmpty(assetField.Alias))
			{
				xmlWriter.WriteAttributeString("Alias", assetField.Alias);
			}
			if (assetField.Multi)
			{
				xmlWriter.WriteAttributeString("Multi", "true");
			}
			if (assetField.Query)
			{
				xmlWriter.WriteAttributeString("Query", "true");
			}
			if (assetField.Refine)
			{
				// refine will not work with out also being queryable
				if (!assetField.Query)
				{
					xmlWriter.WriteAttributeString("Query", "true");
				}
				xmlWriter.WriteAttributeString("Refine", "true");
			}
			if (assetField.Retrieve)
			{
				xmlWriter.WriteAttributeString("Retrieve", "true");
			}
			if (assetField.Safe)
			{
				xmlWriter.WriteAttributeString("Safe", "true");
			}
			if (assetField.Search)
			{
				xmlWriter.WriteAttributeString("Search", "true");
			}
			if (assetField.Sort)
			{
				xmlWriter.WriteAttributeString("Sort", "true");
			}
		}
	}
}