using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines Operations for returning metadata about products.
    /// </summary>
    public interface IProductMetaDataProvider
    {
        /// <summary>
        /// Gets the characteristic data types.
        /// </summary>
        /// <returns></returns>
        IDictionary<byte, string> GetCharacteristicDataTypes();

        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <returns></returns>
        IDictionary<Guid, string> GetScopes();

        /// <summary>
        /// Gets the units of measure.
        /// </summary>
        /// <returns></returns>
        IDictionary<Guid, string> GetUnitsOfMeasure();

        /// <summary>
        /// Gets the type of the characteristic.
        /// </summary>
        /// <returns></returns>
        IDictionary<Guid, string> GetCharacteristicTypes();
    }
}