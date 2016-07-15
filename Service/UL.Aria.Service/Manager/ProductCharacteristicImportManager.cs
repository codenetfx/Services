using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Operations for construction of <see cref="ProductCharacteristic"/> and other properties for a product.
    /// </summary>
    public class ProductCharacteristicImportManager : IProductCharacteristicImportManager
    {
        /// <summary>
        /// Gets the expected name of the type.
        /// </summary>
        /// <value>
        /// The expected name of the type.
        /// </value>
        protected readonly IDictionary<string, ProductFamilyCharacteristicType> ExpectedTypes
            =
            new Dictionary<string, ProductFamilyCharacteristicType>
                {
                    {typeof (ProductFamilyAttribute).Name, ProductFamilyCharacteristicType.Attribute},
                    {typeof (ProductFamilyFeature).Name, ProductFamilyCharacteristicType.Feature}
                };
    
        

        /// <summary>
        /// Fills the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="characteristicCell">The characteristic cell.</param>
        /// <returns>Whether this object handled the cell</returns>
        public bool Fill(Product product, Cell characteristicCell)
        {
            var type = characteristicCell.CharacteristicType();
            if  (!ExpectedTypes.ContainsKey(type))
                return false;
            var productCharacteristic = new ProductCharacteristic();
            productCharacteristic.ProductFamilyCharacteristicType = ExpectedTypes[type];

            productCharacteristic.Value = characteristicCell.StringValue;

            Guid id;
            if (characteristicCell.TryGetProductCharacteristicId(out id))
                productCharacteristic.Id = id;

            var idString = characteristicCell.CharacteristicFamilyId();
            if (Guid.TryParse(idString, out id))
                productCharacteristic.ProductFamilyCharacteristicId = id;
            
            product.Characteristics.Add(productCharacteristic);
            
            return true;
        }
    }
}