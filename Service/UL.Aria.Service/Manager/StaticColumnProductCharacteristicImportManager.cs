using System;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements Operations for construction of <see cref="ProductCharacteristic"/> and other properties for a product.
    /// </summary>
    public class StaticColumnProductCharacteristicImportManager : IProductCharacteristicImportManager
    {
        /// <summary>
        /// Fills the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="characteristicCell">The characteristic cell.</param>
        /// <returns>Whether this object handled the cell</returns>
        public bool Fill(Product product, Cell characteristicCell)
        {
            string type = characteristicCell.CharacteristicType();
            if (ExcelTemplateKeys.StaticCharacteristicType != type)
                return false;
            string id = characteristicCell.CharacteristicFamilyId();
            if (id == ExcelTemplateKeys.ProductNameIdentifier)
            {
                product.Name = characteristicCell.StringValue;
                return true;
            }
            if (id == ExcelTemplateKeys.ProductDescriptionIdentifier)
            {
                product.Description = characteristicCell.StringValue;
                return true;
            }

            if (id == ExcelTemplateKeys.ProductCompanyIdentifier)
            {
                Guid companyId;
                if (Guid.TryParse(characteristicCell.StringValue, out companyId))
                    product.CompanyId = companyId;
                return true;
            }

            return false;
        }
    }
}