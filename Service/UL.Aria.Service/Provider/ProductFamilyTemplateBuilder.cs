using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Implements operations for building documents based on a <see cref="ProductFamily" />.
    /// </summary>
    public class ProductFamilyTemplateBuilder : ProductFamilyDocumentBuilderBase
    {
        private const int EmptyColumn = 0;
        private const int LabelColumn = 1;
        private const int ValueColumn = 2;
        private const int SummaryColumm = 3;
        private const int RefineColumn = 4;
        private const int NotesColumn = 5;
        private readonly IProductMetaDataProvider _productMetaDataProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyTemplateBuilder" /> class.
        /// </summary>
        /// <param name="productMetaDataProvider">The product meta data provider.</param>
        public ProductFamilyTemplateBuilder(IProductMetaDataProvider productMetaDataProvider)
        {
            _productMetaDataProvider = productMetaDataProvider;
        }

        /// <summary>
        ///     Adds the product family.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser"></param>
        public override void AddProductFamily(Workbook workbook, ProductFamily productFamily, ProfileBo creatingUser)
        {
            Worksheet worksheet = AddWorksheet(workbook, productFamily.Name);
            var style = new Style();
            style.Font.Name = "Calibri";
            style.Font.Size = 10;
            var styleFlag = new StyleFlag {FontName = true, FontSize = true};
            worksheet.Cells.ApplyStyle(style, styleFlag);
            

            SetColumnWidths(worksheet);

            worksheet.Cells[1, LabelColumn].PutValue("AUTHOR INFORMATION");
            worksheet.Cells[1, SummaryColumm].PutValue("Summary Results?");
            worksheet.Cells[1, RefineColumn].PutValue("Refinable?");
            worksheet.Cells[1, NotesColumn].PutValue("Notes");
            SetHeaderRowStyle(worksheet, 1);

            worksheet.Cells[3, LabelColumn].PutValue("Name");
            worksheet.Cells[3, ValueColumn].PutValue(creatingUser.DisplayName);
            worksheet.Cells[4, LabelColumn].PutValue("email");
            worksheet.Cells[4, ValueColumn].PutValue(creatingUser.LoginId);
            worksheet.Cells[5, LabelColumn].PutValue("Business Unit");
            worksheet.Cells[6, LabelColumn].PutValue("Subset");
            worksheet.Cells[7, LabelColumn].PutValue("Action");
            worksheet.Cells[7, NotesColumn].PutValue("REPLACE | DELTA");

            worksheet.Cells[9, LabelColumn].PutValue("FAMILY COMMON");
            SetHeaderRowStyle(worksheet, 9);

            worksheet.Cells[11, LabelColumn].PutValue("FamilyName");
            worksheet.Cells[11, ValueColumn].PutValue(productFamily.Name);
            worksheet.Cells[12, LabelColumn].PutValue("FamilyDescription");
            worksheet.Cells[12, ValueColumn].PutValue(productFamily.Description);
            worksheet.Cells[13, LabelColumn].PutValue("Category Tree");
            worksheet.Cells[13, ValueColumn].PutValue(productFamily.CategoryId);
            worksheet.Cells[14, LabelColumn].PutValue("Business Unit");
            worksheet.Cells[15, LabelColumn].PutValue("Subset");
        }

        /// <summary>
        ///     Adds the base characteristics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public override void AddBaseCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo)
        {
            AddCharacteristicsBlock(workbook, characteristics, familyToAddTo, 17, "BASE CHARACTERISTICS",
                                    SetBaseCharacteristicRowStyle);
        }

        /// <summary>
        ///     Adds the other character istics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public override void AddOtherCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo)
        {
            Worksheet worksheet = FindWorksheet(workbook, familyToAddTo.Name);
            var startRow = worksheet.FindLastFilledRow() + 2;

            AddCharacteristicsBlock(workbook, characteristics, familyToAddTo, startRow, "VARIABLE CHARACTERISTICS",
                                    SetOtherCharacteristicRowStyle);
            
        }

        private void SetOtherCharacteristicRowStyle(Worksheet worksheet, int row)
        {
            var style = new Style();
            style.BackgroundColor = Color.FromArgb(255, 237, 237, 237);
            style.SetTwoColorGradient(Color.FromArgb(255, 237, 237, 237), Color.FromArgb(255, 237, 237, 237),
                                      GradientStyleType.DiagonalDown, 2);

            worksheet.Cells[row, LabelColumn].SetStyle(style);
            worksheet.Cells[row, ValueColumn].SetStyle(style);
        }

        /// <summary>
        ///     Adds the dependencies.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dependencies"></param>
        /// <param name="familyToAddto"></param>
        public override void AddDependencies(Workbook workbook, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> dependencies, ProductFamily familyToAddto)
        {
            Worksheet worksheet = FindWorksheet(workbook, familyToAddto.Name);
            var lastRow = worksheet.FindLastFilledRow() + 2;
            worksheet.Cells[lastRow, LabelColumn].PutValue("DEPENDENCIES");
            SetHeaderRowStyle(worksheet, lastRow);
            
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
                var style = new Style();
                style.Font.Name = "Calibri";
                style.Font.Size = 10;
                var styleFlag = new StyleFlag {FontName = true, FontSize = true};
                worksheet.Cells.ApplyStyle(style, styleFlag);
                var lastRow = worksheet.FindLastFilledRow();
                int totalRows = lastRow < 1 ? 1 : lastRow;
                var range = worksheet.Cells.CreateRange(1, SummaryColumm, totalRows, 1);
                range.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Dashed, Color.Black);
                range = worksheet.Cells.CreateRange(1, RefineColumn, totalRows, 1);
                range.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Dashed, Color.Black);
                range = worksheet.Cells.CreateRange(1, NotesColumn, totalRows, 1);
                range.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Dashed, Color.Black);
            }
        }

        private static void SetColumnWidths(Worksheet worksheet)
        {
            worksheet.Cells.Columns[EmptyColumn].Width = 8.43;
            worksheet.Cells.Columns[LabelColumn].Width = 22.29;
            worksheet.Cells.Columns[ValueColumn].Width = 53.43;
            worksheet.Cells.Columns[SummaryColumm].Width = 16.86;
            worksheet.Cells.Columns[RefineColumn].Width = 9.43;
            worksheet.Cells.Columns[NotesColumn].Width = 118.43;
        }

        private static void SetHeaderRowStyle(Worksheet worksheet, int row)
        {
            var style = new Style();
            style.BackgroundColor = Color.FromArgb(255, 255, 255, 204);
            style.SetTwoColorGradient(Color.FromArgb(255, 255, 255, 204), Color.FromArgb(255, 255, 255, 204),
                                      GradientStyleType.DiagonalDown, 2);

            worksheet.Cells[row, LabelColumn].SetStyle(style);
            worksheet.Cells[row, ValueColumn].SetStyle(style);
            worksheet.Cells[row, SummaryColumm].SetStyle(style);
            worksheet.Cells[row, RefineColumn].SetStyle(style);
            worksheet.Cells[row, NotesColumn].SetStyle(style);
            worksheet.Cells.Merge(row, LabelColumn, 1, 2);
        }

        private static void SetBaseCharacteristicRowStyle(Worksheet worksheet, int row)
        {
            var style = new Style();
            style.BackgroundColor = Color.FromArgb(255, 252, 228, 214);
            style.SetTwoColorGradient(Color.FromArgb(255, 252, 228, 214), Color.FromArgb(255, 252, 228, 214),
                                      GradientStyleType.DiagonalDown, 2);

            worksheet.Cells[row, LabelColumn].SetStyle(style);
            worksheet.Cells[row, ValueColumn].SetStyle(style);
        }

        private void AddCharacteristicsBlock(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo, int startRow, string headerLabel, Action<Worksheet, int> styleSetterAction)
        {
            Worksheet worksheet = FindWorksheet(workbook, familyToAddTo.Name);

            worksheet.Cells[startRow, LabelColumn].PutValue(headerLabel);
            SetHeaderRowStyle(worksheet, startRow);
            string val;
            int currentRow = startRow + 2;

            foreach (ProductFamilyCharacteristicDomainEntity characteristic in characteristics)
            {
                worksheet.Cells[currentRow, LabelColumn].PutValue("CharacteristicName");
                worksheet.Cells[currentRow, ValueColumn].PutValue(characteristic.Name);
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("CharacteristicDescription");
                worksheet.Cells[currentRow, ValueColumn].PutValue(characteristic.Description);
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("CharacteristicType");
                if (_productMetaDataProvider.GetCharacteristicTypes()
                                            .TryGetValue(characteristic.CharacteristicTypeId, out val))
                    worksheet.Cells[currentRow, ValueColumn].PutValue(val);
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("Scope");
                if (_productMetaDataProvider.GetScopes().TryGetValue(characteristic.ScopeId, out val))
                    worksheet.Cells[currentRow, ValueColumn].PutValue(val);
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("Required");
                worksheet.Cells[currentRow, ValueColumn].PutValue(characteristic.IsRequired.ToString());
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("UOM");
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("ListOfValues?");
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("AllowAdd");
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("AllowEdit");
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("DataType");
                worksheet.Cells[currentRow, ValueColumn].PutValue(ResolveDataType(characteristic));
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("AllowedValueTypes");
                worksheet.Cells[currentRow, ValueColumn].PutValue("Single");
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("Values");
                worksheet.Cells[currentRow, ValueColumn].PutValue(string.Empty);
                styleSetterAction(worksheet, currentRow);
                ++currentRow;
                worksheet.Cells[currentRow, LabelColumn].PutValue("RequiredForStandards");
                worksheet.Cells[currentRow, ValueColumn].PutValue(characteristic.IsValueRequired.ToString());
                styleSetterAction(worksheet, currentRow);
                currentRow += 2;
            }
        }

        //TODO refactor this.
        private string ResolveDataType(ProductFamilyCharacteristicDomainEntity characteristic)
        {
            PropertyInfo property = characteristic.GetType().GetProperty("DataTypeId");
            if (null != property)
            {
                var b = (ProductFamilyCharacteristicDataType) property.GetValue(characteristic);
                return b.ToString();
            }
            return "String";
        }
    }
}