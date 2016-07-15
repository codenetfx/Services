using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements a stub .
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ProductMetaDataProviderStub : IProductMetaDataProvider
    {
        private readonly Dictionary<byte, string> _characteristicDataTypes = new Dictionary<byte, string>
            {
                {0, "String"},
                {1, "String"},
                {2, "Number"},
                {3, "Date"},
                {4, "Document Reference"}
            };

        private readonly Dictionary<Guid, string> _scopes = new Dictionary<Guid, string>
            {
                {new Guid("4AAA5706-6D86-E211-BCF5-20C9D042ED3E"), "FAMILY"},
                {new Guid("4BAA5706-6D86-E211-BCF5-20C9D042ED3E"), "BUSINESS UNIT"},
                {new Guid("8e270d67-cbd9-e211-92a7-54d9dfe94c0d"), "BUSINESS UNIT" },
                {new Guid("4CAA5706-6D86-E211-BCF5-20C9D042ED3E"), "GLOBAL"}
            };

        private readonly Dictionary<Guid, string> _unitsOfMeasure = new Dictionary<Guid, string>
            {
                {new Guid("C29FBCF1-FE8B-E211-82DA-000C29F434B7"), "GB"},
                {new Guid("637902FC-FE8B-E211-82DA-000C29F434B7"), "TB"},
                {new Guid("ACA18A18-1988-E211-ADAF-000C29F434B7"), "Cu. Ft"},
                {new Guid("FD884E1D-B98C-E211-B2F0-000C29F434B7"), "Amps"},
                {new Guid("D4FB45B2-BE8C-E211-B2F0-000C29F434B7"), "°F"},
                {new Guid("D5FB45B2-BE8C-E211-B2F0-000C29F434B7"), "°C"},
                {new Guid("0DFB9F63-C28C-E211-B2F0-000C29F434B7"), "gal (US)"},
                {new Guid("0EFB9F63-C28C-E211-B2F0-000C29F434B7"), "gal"},
                {new Guid("0FFB9F63-C28C-E211-B2F0-000C29F434B7"), "lt"},
                {new Guid("133D5A5E-C38C-E211-B2F0-000C29F434B7"), "in"},
                {new Guid("CC16B4C3-B2BD-E211-832C-54D9DFE94C0D"), "Lbs"},
                {new Guid("CD16B4C3-B2BD-E211-832C-54D9DFE94C0D"), "Kg"},
                {new Guid("AFB931C0-EABB-E211-8B37-54D9DFE94C0D"), "Horsepower"},
                {new Guid("B0B931C0-EABB-E211-8B37-54D9DFE94C0D"), "Watts"},
                {new Guid("B1B931C0-EABB-E211-8B37-54D9DFE94C0D"), "Kilowatts"},
                {new Guid("4ECB3C36-F5BB-E211-8B37-54D9DFE94C0D"), "Volts"}
            };

        private readonly Dictionary<Guid, string> _characteristicTypes = new Dictionary<Guid, string>
            {
                {new Guid("13128A5F-6D86-E211-BCF5-20C9D042ED3E"), "Construction"},
                {new Guid("14128A5F-6D86-E211-BCF5-20C9D042ED3E"), "Operational"},
                {new Guid("FC08A508-7FBD-E211-832C-54D9DFE94C0D"), "Descriptive"}
                
            };

        /// <summary>
        /// Gets the characteristic data types.
        /// </summary>
        /// <returns></returns>
        public IDictionary<byte, string> GetCharacteristicDataTypes()
        {
            return _characteristicDataTypes;
        }

        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <returns></returns>
        public IDictionary<Guid, string> GetScopes()
        {
            return _scopes;
        }

        /// <summary>
        /// Gets the units of measure.
        /// </summary>
        /// <returns></returns>
        public IDictionary<Guid, string> GetUnitsOfMeasure()
        {
            return _unitsOfMeasure;
        }

        /// <summary>
        /// Gets the type of the characteristic.
        /// </summary>
        /// <returns></returns>
        public IDictionary<Guid, string> GetCharacteristicTypes()
        {
            return _characteristicTypes;
        }
    }
}