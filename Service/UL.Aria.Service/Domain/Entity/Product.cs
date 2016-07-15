using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Product domain entity
    /// </summary>
    public class Product : PrimarySearchEntityBase
    {
        private static readonly Dictionary<string, string> _characteristicTypeConversions = new Dictionary
            <string, string>
            {
                {"Number", typeof (Int32).FullName},
                {"Date", typeof (DateTime).FullName}
            };

        /// <summary>
        ///     Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Product(Guid? id)
            : base(id)
        {
            Type = EntityTypeEnumDto.Product;
            Characteristics = new List<ProductCharacteristic>();
            IsDeleted = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Product" /> class.
        /// </summary>
        public Product() : this(null)
        {
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the product family id.
        /// </summary>
        /// <value>
        ///     The product family id.
        /// </value>
        public Guid ProductFamilyId { get; set; }

        /// <summary>
        ///     Gets or sets the characteristics.
        /// </summary>
        /// <value>
        ///     The characteristics.
        /// </value>
        public IList<ProductCharacteristic> Characteristics { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        public ProductStatus Status { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can delete product.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can delete product; otherwise, <c>false</c>.
        /// </value>
        public bool CanDelete { get; set; }

        /// <summary>
        ///     Gets or sets the date the product was submitted - if it has been.
        /// </summary>
        /// <value>
        ///     The submitted date.
        /// </value>
        public DateTime? SubmittedDateTime { get; set; }

        /// <summary>
        ///     Serializes the parent.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="assetFieldMetadata">The asset field metadata.</param>
        /// <param name="isContainerOnly"></param>
        protected override void SerializeParent(XmlWriter xmlWriter, object parent, IAssetFieldMetadata assetFieldMetadata, bool isContainerOnly = false)
        {
            base.SerializeParent(xmlWriter, parent, assetFieldMetadata, isContainerOnly);

            var product = parent as Product;

            foreach (var characteristic in product.Characteristics)
            {
                WriteCharacteristicSpMetadataElement(xmlWriter, characteristic, assetFieldMetadata, Type);
            }
        }

        /// <summary>
        ///     Writes the characteristic sp metadata element.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="characteristic">The characteristic.</param>
        /// <param name="assetFieldMetadata">The asset field metadata.</param>
        /// <param name="type">The type.</param>
        public static void WriteCharacteristicSpMetadataElement(XmlWriter xmlWriter,
                                                                ProductCharacteristic characteristic,
                                                                IAssetFieldMetadata assetFieldMetadata,
                                                                EntityTypeEnumDto type)
        {
            var assetField = assetFieldMetadata.GetContainerAssetField(type, characteristic.Name) ??
                             new AssetFieldMetadata.AssetField(null)
                                 {
                                     Refine = true,
                                     Retrieve = true,
                                     Search = true,
                                     Query = true,
                                     IsCharacteristic = true
                                 };

            if (assetField.Ignore)
                return;

            string dataTypeName = characteristic.DataType.ToString();

            if (string.IsNullOrEmpty(assetField.Name))
            {
                assetField.Name = "aria" + characteristic.ProductFamilyCharacteristicId.ToString("N");
            }

            WriteElement(xmlWriter, assetField, dataTypeName, characteristic.Value);

            if (characteristic.IsMultivalueAllowed)
            {
                var values = characteristic.ParseMultiValue();

                if (characteristic.IsRangeAllowed)
                    foreach (var value in values)
                    {
                        var ranges = value.Split('-');
                        if (ranges.Length > 1)
                            WriteRange(assetField, xmlWriter, dataTypeName, ranges);
                        else
                            WriteElement(xmlWriter, assetField, dataTypeName, value);
                    }
                else
                    foreach (var value in values)
                        WriteElement(xmlWriter, assetField, dataTypeName, value);
            }
            else if (characteristic.IsRangeAllowed)
            {
                var ranges = characteristic.Value.Split('-');
                WriteRange(assetField, xmlWriter, dataTypeName, ranges);
            }
        }

        private static void WriteRange(AssetFieldMetadata.AssetField assetField, XmlWriter xmlWriter, string dataTypeName, string[] ranges)
        {
            var assetFieldName = assetField.Name;

            assetField.Name = assetFieldName + "Min";
            WriteElement(xmlWriter, assetField, dataTypeName, ranges[0]);

            if (ranges.Length > 1)
            {
                assetField.Name = assetFieldName + "Max";
                WriteElement(xmlWriter, assetField, dataTypeName, ranges[1]);
            }

            assetField.Name = assetFieldName;
        }

        private static void WriteElement(XmlWriter xmlWriter, AssetFieldMetadata.AssetField assetField, string dataTypeName, string value)
        {
            if (assetField.SuppressEmpty && String.IsNullOrWhiteSpace(value))
                return;
            xmlWriter.WriteStartElement(assetField.Name);
            xmlWriter.WriteAttributeString("TypeName", GetCharacteristicSpTypeName(dataTypeName));

            WriteSpAttributes(xmlWriter, assetField);

            xmlWriter.WriteString(value);
            xmlWriter.WriteEndElement();
        }

        private static string GetCharacteristicSpTypeName(string dataTypeName)
        {
            string typeName = "System.String";

            if (_characteristicTypeConversions.ContainsKey(dataTypeName))
                typeName = _characteristicTypeConversions[dataTypeName];

            return typeName;
        }
    }
}