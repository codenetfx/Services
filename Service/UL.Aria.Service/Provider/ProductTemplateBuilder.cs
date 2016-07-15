using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aspose.Cells;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for building a Product template based on a given <see cref="ProductFamily"/>
    /// </summary>
    public class ProductTemplateBuilder : ProductFamilyDocumentBuilderBase
    {
        
        private int _nextColumn;

        /// <summary>
        /// Adds the product family.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser"></param>
        public override void AddProductFamily(Workbook workbook, ProductFamily productFamily, ProfileBo creatingUser)
        {
            AddWorksheet(workbook, productFamily.Name);
        }

        /// <summary>
        /// Adds the base characteristics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public override void AddBaseCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo)
        {
            var worksheet = FindWorksheet(workbook, familyToAddTo.Name);
            int i = 0;
            i = AddColumn(worksheet, i, familyToAddTo.Id.ToString(), "Product Family", ExcelTemplateKeys.ProductFamilyIdentifier, ProductFamilyCharacteristicDataType.String.ToString(), ExcelTemplateKeys.StaticCharacteristicType);
            i = AddColumn(worksheet, i, ExcelTemplateKeys.ProductCompanyIdentifier, "Product Company", ExcelTemplateKeys.ProductCompanyIdentifier, ProductFamilyCharacteristicDataType.String.ToString(), ExcelTemplateKeys.StaticCharacteristicType);
            i = AddColumn(worksheet, i, ExcelTemplateKeys.ProductNameIdentifier, "Product Name", ExcelTemplateKeys.ProductNameIdentifier, ProductFamilyCharacteristicDataType.String.ToString(), ExcelTemplateKeys.StaticCharacteristicType);
            i = AddColumn(worksheet, i, ExcelTemplateKeys.ProductDescriptionIdentifier, ExcelTemplateKeys.ProductDescriptionIdentifier, ExcelTemplateKeys.ProductDescriptionIdentifier, ProductFamilyCharacteristicDataType.String.ToString(), ExcelTemplateKeys.StaticCharacteristicType);
            var productMetaDataProvider = new ProductMetaDataProviderStub();
            var characteristicDataTypes = productMetaDataProvider.GetCharacteristicDataTypes();
            var characteristicTypes = productMetaDataProvider.GetCharacteristicTypes();
            foreach (var characteristic in characteristics.OrderBy(x => x, new CharacteristicTypeComparer(characteristicTypes)).ThenBy(x => x.SortOrder).ThenBy(x => x.Name))
            {
                var dataType = GetCharacteristicDataType(characteristicDataTypes, characteristic);
                i = AddColumn(worksheet, i, characteristic.Id.ToString(), characteristic.Name, characteristicTypes[characteristic.CharacteristicTypeId], dataType, characteristic.GetType().Name);
            }
            _nextColumn = i;
        }

        private class CharacteristicTypeComparer : IComparer<ProductFamilyCharacteristicDomainEntity>
        {
            private readonly IDictionary<Guid, string> _characteristicTypes;

            public CharacteristicTypeComparer(IDictionary<Guid, string> characteristicTypes)
            {
                _characteristicTypes = characteristicTypes;
            }

            private static readonly Guid descriptiveId = new Guid("FC08A508-7FBD-E211-832C-54D9DFE94C0D");
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <returns>
            /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
            public int Compare(ProductFamilyCharacteristicDomainEntity x, ProductFamilyCharacteristicDomainEntity y)
            {
                if (x == null)
                    return -1;
                if (y == null)
                    return 1;
                if (x.CharacteristicTypeId == y.CharacteristicTypeId)
                    return 0;
                if (x.CharacteristicTypeId == descriptiveId)
                    return -1;
                if (_characteristicTypes.ContainsKey(x.CharacteristicTypeId))
                {
                    if (_characteristicTypes.ContainsKey(y.CharacteristicTypeId))
                    {
                        return _characteristicTypes[x.CharacteristicTypeId].CompareTo(_characteristicTypes[y.CharacteristicTypeId]);
                    }
                    return -1;
                }
                return 1;
            }
        }

        private string GetCharacteristicDataType(IDictionary<byte, string> characteristicDataTypes, ProductFamilyCharacteristicDomainEntity characteristic)
        {
            var dataType = "";
            if (characteristic is ProductFamilyFeature)
            {
                dataType = "list";
            }
            else if (characteristic is ProductFamilyAttribute)
            {
                dataType = ((ProductFamilyAttribute)characteristic).DataTypeId.ToString();
            }

            return dataType;
        }

        private int AddColumn(Worksheet worksheet, int i, string columnId, string columnName, string type, string dataType, string characteristicType)
        {
            var style = worksheet.Cells.Columns[i].Style;
            style.IsLocked = false;
            var styleflag = new StyleFlag();
            styleflag.Locked = true;
            worksheet.Cells.Columns[i].ApplyStyle(style, styleflag);

            var cell = worksheet.Cells[ExcelTemplateKeys.CharacteristicIdRow, i];
            style = GetHiddenStyle();
            cell.SetStyle(style);
            cell.PutValue(columnId);

            cell = worksheet.Cells[ExcelTemplateKeys.CharacteristicScopeRow, i];
            cell.PutValue(type);
            cell.SetStyle(GetHeaderSyle());

            cell = worksheet.Cells[ExcelTemplateKeys.CharacteristicDataTypeRow, i];
            cell.PutValue(dataType);
            cell.SetStyle(GetHeaderSyle());

            cell = worksheet.Cells[ExcelTemplateKeys.CharacteristicTypeRow, i];
            cell.PutValue(characteristicType);
            cell.SetStyle(GetHeaderSyle());

            cell = worksheet.Cells[ExcelTemplateKeys.CharacteristicNameRow, i];
            cell.PutValue(columnName);
            cell.SetStyle(GetHeaderSyle());
    
            ++i;
            return i;
        }

        private static Style GetHiddenStyle()
        {
            var style = new Style();
            style.Font.Size = 1;
            style.BackgroundColor = Color.White;
            style.ForegroundColor = Color.Gray;
            return style;
        }

        private Style GetHeaderSyle()
        {
            var style = new Style();
            style.Font.Color = Color.Black;
            style.Font.IsBold = true;
            return style;
        }

        /// <summary>
        /// Adds the other character istics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public override void AddOtherCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo)
        {
            var worksheet = FindWorksheet(workbook, familyToAddTo.Name);
            int i = _nextColumn;
            var productMetaDataProvider = new ProductMetaDataProviderStub();
            var characteristicDataTypes = productMetaDataProvider.GetCharacteristicDataTypes();
            var characteristicTypes = productMetaDataProvider.GetCharacteristicTypes();
            foreach (var characteristic in characteristics)
            {
                var dataType = GetCharacteristicDataType(characteristicDataTypes, characteristic);
                i = AddColumn(worksheet, i, characteristic.Id.ToString(), characteristic.Name, characteristicTypes[characteristic.CharacteristicTypeId], dataType, characteristic.GetType().Name);
            }
            _nextColumn = i;
        }

        /// <summary>
        /// Adds the dependencies.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dependencies"></param>
        /// <param name="familyToAddto"></param>
        public override void AddDependencies(Workbook workbook, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> dependencies, ProductFamily familyToAddto)
        {
        }

        /// <summary>
        /// Finalizes the style.
        /// </summary>
        /// <param name="workbook"></param>
        protected override void FinalizeStyle(Workbook workbook)
        {
            foreach (Worksheet worksheet in
                from object worksheetItem in workbook.Worksheets select worksheetItem as Worksheet)
            {
                FinalizeSheetStyle(worksheet);
            }
        }

        /// <summary>
        /// Finalizes the sheet style.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        protected virtual void FinalizeSheetStyle(Worksheet worksheet)
        {
            var style = new Style();
            style.Font.Name = "Calibri";
            style.Font.Size = 8;
            var styleFlag = new StyleFlag {FontName = true, FontSize = true};
            worksheet.Cells.ApplyStyle(style, styleFlag);
            worksheet.AutoFitColumns();
            worksheet.Cells.Rows[ExcelTemplateKeys.CharacteristicIdRow].Hide().Lock();
            worksheet.Cells.Rows[ExcelTemplateKeys.CharacteristicScopeRow].Hide().Lock();
            worksheet.Cells.Rows[ExcelTemplateKeys.CharacteristicDataTypeRow].Lock();
            worksheet.Cells.Rows[ExcelTemplateKeys.CharacteristicTypeRow].Hide().Lock();
            worksheet.Cells.Rows[ExcelTemplateKeys.CharacteristicNameRow].Lock();
            worksheet.Cells.Columns[0].Hide().Lock();
            worksheet.Cells.Columns[1].Hide().Lock();
            worksheet.Protect(ProtectionType.All);
            worksheet.FirstVisibleRow = ExcelTemplateKeys.CharacteristicDataTypeRow;
        }
    }
}