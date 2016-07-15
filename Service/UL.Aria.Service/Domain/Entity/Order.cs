using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class Order
    /// </summary>
    public class Order : IncomingOrder
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Order" /> class.
        /// </summary>
        public Order()
        {
            Type = EntityTypeEnumDto.Order;
        }

		/// <summary>
		/// Gets the incoming order service line industry code.
		/// </summary>
		/// <value>
		/// The incoming order service line industry code.
		/// </value>
		public string IncomingOrderServiceLineIndustryCode
		{
			get
			{
				AssureServicelinesMetaParsed();
				return this._serviceLineMetaItemsDictionary[AssetFieldNames.AriaOrderIndustryCode];
			}
		}

		/// <summary>
		/// Gets the incoming order service line service code.
		/// </summary>
		/// <value>
		/// The incoming order service line service code.
		/// </value>
		public string IncomingOrderServiceLineServiceCode
		{
			get
			{
				AssureServicelinesMetaParsed();
				return this._serviceLineMetaItemsDictionary[AssetFieldNames.AriaOrderServiceCode];
			}
		}

		/// <summary>
		/// Gets the incoming order service line location code.
		/// </summary>
		/// <value>
		/// The incoming order service line location code.
		/// </value>
		public string IncomingOrderServiceLineLocationCode
		{
			get
			{
				AssureServicelinesMetaParsed();
				return this._serviceLineMetaItemsDictionary[AssetFieldNames.AriaOrderLocationCode];
			}
		}

		private Dictionary<string, string> _serviceLineMetaItemsDictionary = null;
		private static readonly object PadLock = new object();

		private void AssureServicelinesMetaParsed()
		{
			string sharePointDelimiter = AssetFieldNames.SharePointMultivalueSeparator.ToString();
			Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
			Dictionary<string, string> finalDict = new Dictionary<string, string>();

			if (_serviceLineMetaItemsDictionary == null)
			{
				lock (PadLock)
				{
					if (_serviceLineMetaItemsDictionary == null)
					{
						dict.Add(AssetFieldNames.AriaOrderIndustryCode, new List<string>());
						dict.Add(AssetFieldNames.AriaOrderLocationCode, new List<string>());
						dict.Add(AssetFieldNames.AriaOrderServiceCode, new List<string>());

						if (ServiceLines != null && ServiceLines.Any())
						{
							foreach (var line in ServiceLines)
							{
								if (!string.IsNullOrWhiteSpace(line.IndustryCode) && !dict[AssetFieldNames.AriaOrderIndustryCode].Any(x=> string.Equals(x,line.IndustryCode, StringComparison.OrdinalIgnoreCase)))
									dict[AssetFieldNames.AriaOrderIndustryCode].Add(line.IndustryCode);

								if (!string.IsNullOrWhiteSpace(line.LocationCode) && !dict[AssetFieldNames.AriaOrderLocationCode].Any(x => string.Equals(x, line.LocationCode, StringComparison.OrdinalIgnoreCase)))
									dict[AssetFieldNames.AriaOrderLocationCode].Add(line.LocationCode);

								if (!string.IsNullOrWhiteSpace(line.ServiceCode) && !dict[AssetFieldNames.AriaOrderServiceCode].Any(x => string.Equals(x, line.ServiceCode, StringComparison.OrdinalIgnoreCase)))
									dict[AssetFieldNames.AriaOrderServiceCode].Add(line.ServiceCode);

							}
						}

						finalDict.Add(AssetFieldNames.AriaOrderIndustryCode, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaOrderIndustryCode]));
						finalDict.Add(AssetFieldNames.AriaOrderLocationCode, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaOrderLocationCode]));
						finalDict.Add(AssetFieldNames.AriaOrderServiceCode, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaOrderServiceCode]));
						// ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
						this._serviceLineMetaItemsDictionary = finalDict;

					}
				}
			}
		}

    }
}