using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Auditing
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DocumentManagerAuditInterceptor : AuditInterceptionBehaviorBase<IDictionary<string, string>, IDictionary<string, string>>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentManagerAuditInterceptor" /> class.
        /// </summary>
        /// <param name="historyProvider">The history provider.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="assetProvider">The asset provider.</param>
        public DocumentManagerAuditInterceptor(IHistoryProvider historyProvider, IPrincipalResolver principalResolver, IProfileManager profileManager, IAssetProvider assetProvider)
            : base(historyProvider, principalResolver, profileManager)
        {
            _assetProvider = assetProvider;
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Guid.
        /// </returns>
        protected override Guid GetEntityId(IDictionary<string, string> entity)
        {
            return new Guid(entity[AssetFieldNames.SharePointAssetId]);
        }

        /// <summary>
        /// when implemented in a derived class, returns a DataContract Serializable
        /// object to be stored as the audit details.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected override IDictionary<string, string> ConvertToDto(IDictionary<string, string> entity)
        {
            return entity;
        }

        /// <summary>
        /// XMLs the serialize.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        protected override string XmlSerialize(IDictionary<string, string> dto)
        {
            StringWriter stringWriter = null;
            try
            {
                stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = null;
                try
                {
                    xmlWriter = new XmlTextWriter(stringWriter);
                    var xsn = new XmlSerializerNamespaces();
                    xsn.Add("", "");
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("ariaAssetMetadata");

                    foreach (var kvp in dto)
                    {
                        xmlWriter.WriteStartElement(kvp.Key);
                        xmlWriter.WriteValue(kvp.Value);
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    return stringWriter.ToString();
                }
                finally
                {
                    if (xmlWriter != null)
                    {
                        xmlWriter.Dispose();
                        stringWriter = null;
                    }
                }
            }
            finally
            {
                if (stringWriter != null)
                    stringWriter.Dispose();
            }
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>
        /// T.
        /// </returns>
        protected override IDictionary<string, string> GetEntity(Guid entityId)
        {
            var metaData =_assetProvider.Fetch(entityId);
            metaData[AssetFieldNames.SharePointAssetId] = entityId.ToString();
            metaData[AssetFieldNames.AriaLastModifiedBy] = _principalResolver.Current.Identity.Name;
            metaData[AssetFieldNames.AriaLastModifiedOn] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            return metaData;
        }
    }
}
