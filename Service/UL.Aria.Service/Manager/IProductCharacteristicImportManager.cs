using System.Web;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines Operations for construction of <see cref="ProductCharacteristic"/> and other properties for a product.
    /// </summary>
    public interface IProductCharacteristicImportManager
    {
        /// <summary>
        /// Fills the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="characteristicCell">The characteristic cell.</param>
        /// <returns>Whether this object handled the cell</returns>
        bool Fill(Product product, Cell characteristicCell);
    }
}