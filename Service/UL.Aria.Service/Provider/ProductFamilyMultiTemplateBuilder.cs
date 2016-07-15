using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Implements a product family template builder that uses several sheets to present the family.
    /// </summary>
    public class ProductFamilyMultiTemplateBuilder : ProductFamilyDocumentBuilderBase
    {
        private readonly IProductMetaDataProvider _productMetaDataProvider;
        private readonly IProductFamilyFeatureProvider _featureProvider;
        private readonly IValidationBuilder _unitOfMeasurevalidationBuilder;
        private readonly IValidationBuilder _valueTypeValidationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyMultiTemplateBuilder" /> class.
        /// </summary>
        /// <param name="productMetaDataProvider">The product meta data provider.</param>
        /// <param name="featureProvider">The feature provider.</param>
        /// <param name="unitOfMeasurevalidationBuilder">The unit of measure builder.</param>
        /// <param name="valueTypeValidationBuilder">The familyId type validation builder.</param>
        public ProductFamilyMultiTemplateBuilder(IProductMetaDataProvider productMetaDataProvider, IProductFamilyFeatureProvider featureProvider, IValidationBuilder unitOfMeasurevalidationBuilder, IValidationBuilder valueTypeValidationBuilder)
        {
            _productMetaDataProvider = productMetaDataProvider;
            _featureProvider = featureProvider;
            _unitOfMeasurevalidationBuilder = unitOfMeasurevalidationBuilder;
            _valueTypeValidationBuilder = valueTypeValidationBuilder;
        }

        /// <summary>
        ///     Finalizes the style.
        /// </summary>
        /// <param name="workbook"></param>
        protected override void FinalizeStyle(Workbook workbook)
        {
            Worksheet worksheet = workbook.Worksheets[ExcelTemplateKeys.AttributesAndFeatures];
            _unitOfMeasurevalidationBuilder.Build(workbook, worksheet.Cells.Find(ExcelTemplateKeys.UoMLabel, worksheet.Cells[0,0], new FindOptions()).Column ,400);
            _valueTypeValidationBuilder.Build(workbook, worksheet.Cells.Find(ExcelTemplateKeys.AllowedValueTypeLabel, worksheet.Cells[0, 0], new FindOptions()).Column, 400);
            var sheet = workbook.Worksheets[0];
            var headerStyle = new Style();
            headerStyle.Font.Name = "Calibri";
            headerStyle.Font.Size = 10;
            headerStyle.SetTwoColorGradient(Color.FromArgb(177, 8, 32), Color.FromArgb(142, 5, 24), GradientStyleType.Horizontal, 1);
            headerStyle.Font.Color = Color.White;
            var headerStyleFlag = new StyleFlag { FontName = true, FontSize = true , FontColor = true, CellShading = true};
            var range = sheet.Cells.CreateRange(0, 0, 1, 2);
            range.ApplyStyle(headerStyle, headerStyleFlag);
            range = sheet.Cells.CreateRange(7, 0, 1, 2);
            range.ApplyStyle(headerStyle, headerStyleFlag);
            
            var style = new Style();
            style.Font.Name = "Calibri";
            style.Font.Size = 10;
            var styleFlag = new StyleFlag { FontName = true, FontSize = true };
            sheet.Cells.ApplyStyle(style, styleFlag);
            
            sheet = worksheet;
            range = sheet.Cells.CreateRange(0, 0, 1, 14);
            range.ApplyStyle(headerStyle, headerStyleFlag);
            sheet.Cells.ApplyStyle(style, styleFlag);
            sheet.AutoFitColumns();

            sheet = workbook.Worksheets[ExcelTemplateKeys.Dependencies];
            range = sheet.Cells.CreateRange(0, 0, 1, 5);
            range.ApplyStyle(headerStyle, headerStyleFlag);
            sheet.Cells.ApplyStyle(style, styleFlag);
            sheet.AutoFitColumns();
        }

        /// <summary>
        ///     Adds the product family.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser"></param>
        public override void AddProductFamily(Workbook workbook, ProductFamily productFamily, ProfileBo creatingUser)
        {
            Worksheet worksheet = workbook.Worksheets.Add(ExcelTemplateKeys.FamilyBasics);
            Cell familyHeader = worksheet.Cells[0, 0]
                .AddTwoColumn("Family", productFamily.Id.Value.ToString(), true)
                ;
            Cell authorHeader = familyHeader
                .AddTwoColumn(ExcelTemplateKeys.FamilyName, productFamily.Name)
                .AddTwoColumn(ExcelTemplateKeys.FamilyDescription, productFamily.Description)
                .AddTwoColumn(ExcelTemplateKeys.CategoryTree, productFamily.CategoryId.ToString())
                .AddTwoColumn(ExcelTemplateKeys.BusinessUnit, productFamily.BusinessUnitId + string.Empty)
                .AddTwoColumn(ExcelTemplateKeys.ActionLabel, "Update")
                .AddTwoColumn("Status", productFamily.Status.ToString())
                .AddTwoColumn("Author", creatingUser.Id.ToString());
            authorHeader
                .AddTwoColumn(ExcelTemplateKeys.AuthorEmail, creatingUser.LoginId)
                .AddTwoColumn(ExcelTemplateKeys.AuthorName, creatingUser.DisplayName);
            
            worksheet.AutoFitColumns(0, 1);
        }

        

        /// <summary>
        ///     Adds the base characteristics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public override void AddBaseCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo)
        {
            Cell lastCell;
            var worksheet = AddAttributesAndFeaturesSheet(workbook, out lastCell);
            AddCharacteristicsToTable(characteristics, worksheet, lastCell, familyToAddTo.Id.Value, familyToAddTo.BusinessUnitId.GetValueOrDefault());
        }

        private static Worksheet AddAttributesAndFeaturesSheet(Workbook workbook, out Cell lastCell)
        {
            Worksheet worksheet = workbook.Worksheets.Add(ExcelTemplateKeys.AttributesAndFeatures);
            lastCell = worksheet.Cells[0, 0].AddTableHeader(ExcelTemplateKeys.CharacteristicIdLabel, true)
                                            .AddTableHeader(ExcelTemplateKeys.ActionLabel)
                                            .AddTableHeader(ExcelTemplateKeys.NameLabel)
                                            .AddTableHeader(ExcelTemplateKeys.DescriptionLabel)
                                            .AddTableHeader(ExcelTemplateKeys.TypeLabel)
                                            .AddTableHeader(ExcelTemplateKeys.ScopeLabel)
                                            .AddTableHeader(ExcelTemplateKeys.RequiredLabel)
                                            .AddTableHeader(ExcelTemplateKeys.ValueRequiredLabel)
                                            .AddTableHeader(ExcelTemplateKeys.UoMLabel)
                                            .AddTableHeader(ExcelTemplateKeys.LoVLabel)
                                            .AddTableHeader(ExcelTemplateKeys.ListOpenLabel)
                                            .AddTableHeader(ExcelTemplateKeys.DataTypeLabel)
                                            .AddTableHeader(ExcelTemplateKeys.AllowedValueTypeLabel)
                                            .AddTableHeader(ExcelTemplateKeys.AllowedValueLabel);
            return worksheet;
        }

        internal Cell AddCharacteristicsToTable(IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, Worksheet worksheet, Cell lastCell, Guid familyId, Guid businessUnitId)
        {
            Cell internalLastCell = lastCell;
            foreach (ProductFamilyCharacteristicDomainEntity characteristic in characteristics)
            {
                internalLastCell = worksheet.Cells[internalLastCell.Row + 1, 0];
                if (characteristic is ProductFamilyFeature)
                    AddCharacteristicToTable(internalLastCell, (ProductFamilyFeature) characteristic, familyId, businessUnitId);
                else
                    AddCharacteristicToTable(internalLastCell, (ProductFamilyAttribute) characteristic, familyId, businessUnitId);
            }
            return internalLastCell;
        }

        internal Cell AddCharacteristicToTable(Cell lastCell, ProductFamilyAttribute characteristic, Guid familyId, Guid businessUnitId)
        {
            lastCell.AddTableValue(characteristic.Id.Value.ToString(), true)
                    .AddTableValue("Update")
                    .AddTableValue(characteristic.Name)
                    .AddTableValue(characteristic.Description)
                    .AddTableValue(_productMetaDataProvider.GetCharacteristicTypes()[characteristic.CharacteristicTypeId])
                    .AddTableValue(GetScope(characteristic.ScopeId, familyId, businessUnitId))
                    .AddTableValue(characteristic.IsRequired.ToYesNo())
                    .AddTableValue(characteristic.IsValueRequired.ToYesNo())
                    .AddTableValue(characteristic.UnitOfMeasureId.HasValue ? characteristic.UnitOfMeasureId.ToString() : "UOM")
                    .AddTableValue("No")
                    .AddTableValue("No")
                    .AddTableValue(characteristic.DataTypeId.ToString())
                    .AddTableValue(String.Join(",", characteristic.Options.Where(x => x.Name == ProductFamilyCharacteristicOptionName.AllowValueTypes).Select(y => y.Value)))
                    .AddTableValue(string.Empty);
            return lastCell;
        }

        internal Cell AddCharacteristicToTable(Cell lastCell, ProductFamilyFeature characteristic, Guid familyId, Guid businessUnitId)
        {
            var values = _featureProvider.FindAllowedValues(characteristic.Id.Value, familyId);
            lastCell.AddTableValue(characteristic.Id.Value.ToString(), true)
                    .AddTableValue("Update")
                    .AddTableValue(characteristic.Name)
                    .AddTableValue(characteristic.Description)
                    .AddTableValue(_productMetaDataProvider.GetCharacteristicTypes()[characteristic.CharacteristicTypeId])
                    .AddTableValue(GetScope(characteristic.ScopeId, familyId, businessUnitId))
                    .AddTableValue(characteristic.IsRequired.ToYesNo())
                    .AddTableValue(characteristic.IsValueRequired.ToYesNo())
                    .AddTableValue(characteristic.UnitOfMeasureId.HasValue ? characteristic.UnitOfMeasureId.ToString() : "UOM")
                    .AddTableValue("Yes")
                    .AddTableValue(characteristic.AllowChanges.ToYesNo())
                    .AddTableValue("list")
                    .AddTableValue(String.Join(",", characteristic.Options.Where(x => x.Name == ProductFamilyCharacteristicOptionName.AllowValueTypes).Select(y => y.Value)))
                    .AddTableValue(string.Join("|", values.Select(x=>x.FeatureValue.Value)));
            return lastCell;
        }

        internal static string GetScope(Guid scopeId, Guid familyId, Guid businessUnitId)
        {
            
            if (scopeId == familyId)
                return "FAMILY";
            if (scopeId == businessUnitId)
                return "BUSINESS UNIT";
            if (scopeId == new Guid(ExcelTemplateKeys.GlobalScopeId))
                return "GLOBAL";
            return "FAMILY";
        }
        /// <summary>
        ///     Adds the other character istics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public override void AddOtherCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo)
        {
            Worksheet worksheet = workbook.Worksheets[ExcelTemplateKeys.AttributesAndFeatures];

            Cell lastCell = worksheet.Cells[worksheet.FindLastFilledRow(), 0];
            AddCharacteristicsToTable(characteristics, worksheet, lastCell, familyToAddTo.Id.Value, familyToAddTo.BusinessUnitId.GetValueOrDefault());
        }

        /// <summary>
        ///     Adds the dependencies.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dependencies"></param>
        /// <param name="familyToAddto"></param>
        public override void AddDependencies(Workbook workbook, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> dependencies, ProductFamily familyToAddto)
        {
            Worksheet worksheet = workbook.Worksheets.Add(ExcelTemplateKeys.Dependencies);

            var lastCell = worksheet.Cells[0, 0]
                                                 .AddTableHeader(ExcelTemplateKeys.DependencyParentId, true)
                                                 .AddTableHeader(ExcelTemplateKeys.DependencyParentValues)
                                                 .AddTableHeader(ExcelTemplateKeys.DependencyChildId)
                                                 .AddTableHeader(ExcelTemplateKeys.DependencyChildValues)
                                                 .AddTableHeader(ExcelTemplateKeys.ActionLabel);

            foreach (var mapping in dependencies)
            {
                lastCell = worksheet.Cells[lastCell.Row + 1, 0];
                lastCell.AddTableValue(mapping.ParentAssocation.CharacteristicId.ToString(), true)
                                   .AddTableValue(string.Join("|", mapping.ParentValues.Select(x => x.FeatureValue.Value)))
                                   .AddTableValue(mapping.ChildAssocation.CharacteristicId.ToString())
                                   .AddTableValue(string.Join("|", mapping.ChildValues.Select(x => x.FeatureValue.Value)))
                                   .AddTableValue("Update");
            }
        }
    }
}