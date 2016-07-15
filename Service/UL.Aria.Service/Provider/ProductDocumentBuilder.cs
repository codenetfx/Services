using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for building documents based on a product.
    /// </summary>
    public class ProductDocumentBuilder : ProductTemplateBuilder, IProductDocumentBuilder
    {
        /// <summary>
        /// Builds the specified product family.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser">The creating user.</param>
        /// <param name="baseCharacteristics">The base characteristics.</param>
        /// <param name="variableCharacteristics">The variable characteristics.</param>
        /// <param name="dependencyMapping"></param>
        /// <returns></returns>
        public Stream Build(IEnumerable<Product> products, ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> dependencyMapping)
        {
            var workbook = base.InitializeDocument();

            AddProductFamily(workbook, productFamily, creatingUser);
            AddBaseCharacteristics(workbook, baseCharacteristics, productFamily);
            AddOtherCharacteristics(workbook, variableCharacteristics, productFamily);
            AddDependencies(workbook, dependencyMapping, productFamily);
            AddProductDetails(workbook, products, productFamily);
            return FinalizeDocument(workbook);
        }

        internal void AddProductDetails(Workbook workbook, IEnumerable<Product> products, ProductFamily familyToAddTo)
        {
            var worksheet = FindWorksheet(workbook, familyToAddTo.Name);
            if ("ProductId" != worksheet.Cells[1, 1].StringValue)
            {
                worksheet.Cells.InsertColumn(0, true);
                worksheet.Cells[1, 0].PutValue("ProductId");
            }
            bool isFirst = true;
            var currentRow = worksheet.FindLastFilledRow() +1;
            foreach (var product in products)
            {
                worksheet.Cells[currentRow, 0].PutValue(product.Id.ToString());
                PutValue(worksheet, ExcelTemplateKeys.ProductFamilyIdentifier, currentRow, familyToAddTo.Name);
                PutValue(worksheet, ExcelTemplateKeys.ProductCompanyIdentifier, currentRow, product.CompanyId.ToString());
                PutValue(worksheet, ExcelTemplateKeys.ProductNameIdentifier, currentRow, product.Name);
                PutValue(worksheet, ExcelTemplateKeys.ProductDescriptionIdentifier, currentRow, product.Description);
                foreach (var characteristic in product.Characteristics)
                {
                    var columnIdentifier = characteristic.ProductFamilyCharacteristicId.ToString();
                    
                    var stringValue = characteristic.Value;
                    var columnIndex = PutValue(worksheet, columnIdentifier, currentRow, stringValue);
                    if (columnIndex != -1)
                    {
                        var idColumnIndex = 0;
                        if (isFirst)
                        {
                            idColumnIndex = 3;
                            worksheet.Cells.InsertColumn(idColumnIndex, true);
                            worksheet.Cells[ExcelTemplateKeys.CharacteristicIdRow, idColumnIndex].PutValue(ExcelTemplateKeys.IdIdentifier);
                            worksheet.Cells[ExcelTemplateKeys.CharacteristicTypeRow, idColumnIndex].PutValue(characteristic.ProductFamilyCharacteristicId.ToString());
                            ++columnIndex;
                        }
                        else
                        {
                            idColumnIndex = worksheet.FindCharacteristicIdColumn(characteristic.ProductFamilyCharacteristicId.ToString());
                        }
                        worksheet.Cells[currentRow, idColumnIndex].PutValue(characteristic.Id.ToString());
                    }
                }
                isFirst = false;
                ++currentRow;
            }
        }

        internal static int PutValue(Worksheet worksheet, string columnIdentifier, int currentRow, string stringValue)
        {
            int columnIndex = 0;
            if (!TryFindColumn(worksheet, columnIdentifier, out columnIndex))
                return -1;
            worksheet.Cells[currentRow, columnIndex].PutValue(stringValue);
            return columnIndex;
        }

        private static bool TryFindColumn(Worksheet worksheet, string columnIdentifier, out int columnIndex)
        {
            columnIndex = 0;
            var cell = worksheet.Cells.Find(columnIdentifier, null, new FindOptions());
            if (null == cell)
                return false;
            columnIndex = cell.Column;
            return true;
        }

        /// <summary>
        /// Finalizes the sheet style.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        protected override void FinalizeSheetStyle(Worksheet worksheet)
        {
            base.FinalizeSheetStyle(worksheet);
            worksheet.Cells.Columns[1].Hide().Lock();
            worksheet.Cells.Columns[2].Hide().Lock();
            int columnIndex = 3;
            string text;
            text = worksheet.Cells[0, columnIndex].StringValue;
            while (!string.IsNullOrEmpty(text))
            {
                if (text == ExcelTemplateKeys.IdIdentifier)
                {
                    worksheet.Cells.Columns[columnIndex].Hide().Lock();
                }
                ++columnIndex;
                text = worksheet.Cells[0, columnIndex].StringValue;                
            }
        }
    }
}