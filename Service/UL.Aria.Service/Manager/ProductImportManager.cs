using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operatiosn for importing <see cref="Product"/> entities from documents.
    /// </summary>
    public class ProductImportManager : IProductImportManager
    {
        private readonly IEnumerable<IProductCharacteristicImportManager> _factories;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductImportManager"/> class.
        /// </summary>
        /// <param name="factories">The factories.</param>
        public ProductImportManager(IEnumerable<IProductCharacteristicImportManager> factories)
        {
            _factories = factories;
        }

        /// <summary>
        /// Imports the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public IEnumerable<ProductUploadResult> Import(Stream stream)
        {
           return Import(new Workbook(stream));
        }

        internal IEnumerable<ProductUploadResult> Import(Workbook workbook)
        {
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                var stringValue = worksheet.Cells[0, 1].StringValue;
                Guid productFamilyId;
                if (!Guid.TryParse(stringValue, out productFamilyId))
                { 
                    stringValue = worksheet.Cells[0, 0].StringValue;
                    if (!Guid.TryParse(stringValue, out productFamilyId))
                        continue;
                }

                for (int i = ExcelTemplateKeys.CharacteristicNameRow + 1; i <= FindLastFilledRow(worksheet); ++i)
                {
                    yield return BuildProduct(worksheet, worksheet.Cells.Rows[i], productFamilyId);
                }
            
            }
            
        }

        internal ProductUploadResult BuildProduct(Worksheet worksheet, Row productRow, Guid productFamilyId)
        {
            Guid productId;
            Guid.TryParse(productRow[0].StringValue, out productId);
            var product = new Product(productId == Guid.Empty ? null as Guid? : productId)
                {
                    ProductFamilyId = productFamilyId,
                    Characteristics = new List<ProductCharacteristic>(),
                };
	        var lastColumnIndex = worksheet.Cells.Rows[ExcelTemplateKeys.CharacteristicIdRow].LastDataCell.Column;
            
            for (int i = 0; i <= lastColumnIndex; ++i)
            {
                
                if ("ID" == worksheet.Cells[ExcelTemplateKeys.CharacteristicIdRow, i].StringValue)
                {
                    continue;
                }

                foreach (var factory in _factories)
                {
                    if (factory.Fill(product, productRow[i]))
                        break;
                }
                
            }
            var productUploadResult = new ProductUploadResult
            {
                IsValid = true,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                Product = product
            };
            return productUploadResult;
        }

        /// <summary>
        /// Finds the last filled row.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <returns></returns>
        protected int FindLastFilledRow(Worksheet worksheet)
        {
            Cell lastCell = worksheet.Cells.LastCell;
            if (null == lastCell)
                return 1;
            var row = lastCell.Row;
            return row;
        }
    }
}