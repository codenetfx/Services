using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class SharePointContainerSerializer
    /// </summary>
    public class SharePointContainerSerializer : IContainerSerializer
    {
        /// <summary>
        ///     The template version
        /// </summary>
        private const int templateVersion = 1;

        /// <summary>
        ///     Serializes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>System.String.</returns>
        public string Serialize(Container container)
        {
            MemoryStream memoryStream = null;

            try
            {
                memoryStream = new MemoryStream();
                var xmlWriterSettings = new XmlWriterSettings {Encoding = new UTF8Encoding()};
                XmlWriter xmlWriter = null;
                try
                {
                    xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
                    var xsn = new XmlSerializerNamespaces();
                    xsn.Add("", "");
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("ariaContainer");

                    xmlWriter.WriteAttributeString("containerId", container.Id.Value.ToString());
                    xmlWriter.WriteAttributeString("containerType", container.PrimarySearchEntityType);
                    xmlWriter.WriteAttributeString("version", templateVersion.ToString());

                    AddGroups(xmlWriter, container);
                    AddLists(xmlWriter, container);

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    return new UTF8Encoding().GetString(memoryStream.ToArray());
                }
                finally
                {
                    if (xmlWriter != null)
                    {
                        xmlWriter.Dispose();
                        memoryStream = null;
                    }
                }
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
            }
        }

        private void AddGroups(XmlWriter xmlWriter, Container container)
        {
            xmlWriter.WriteStartElement("groups");

            IList<ContainerListPermission> containerListPermissions = new List<ContainerListPermission>();
            foreach (var containerList in container.ContainerLists)
            {
                foreach (var containerListPermission in containerList.Permissions)
                {
                    if (containerListPermissions.FirstOrDefault(x => x.Claim.Type == containerListPermission.Claim.Type && x.Claim.Value == containerListPermission.Claim.Value && x.Claim.ValueType == containerListPermission.Claim.ValueType && x.GroupName == containerListPermission.GroupName) == null)
                        containerListPermissions.Add(containerListPermission);
                }
            }
            var containerListPermissionsGrouped = containerListPermissions.GroupBy(x => x.GroupName);

            foreach (var containerListPermissionGroup in containerListPermissionsGrouped)
            {
                var groupName = containerListPermissionGroup.Key;

                xmlWriter.WriteStartElement("group");
                xmlWriter.WriteAttributeString("name", groupName);

                xmlWriter.WriteStartElement("claims");

                foreach (var containerListPermission in containerListPermissionGroup)
                {
                    xmlWriter.WriteStartElement("claim");
                    xmlWriter.WriteAttributeString("claimType", containerListPermission.Claim.Type);
                    xmlWriter.WriteAttributeString("claimValue",
                                                   containerListPermission.Claim.Value);
                    xmlWriter.WriteAttributeString("claimValueType", containerListPermission.Claim.ValueType);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
        }

        private void AddLists(XmlWriter xmlWriter, Container container)
        {
            xmlWriter.WriteStartElement("lists");

            foreach (var containerList in container.ContainerLists)
            {
                xmlWriter.WriteStartElement("list");
                xmlWriter.WriteAttributeString("name", containerList.Name);
                xmlWriter.WriteAttributeString("type", containerList.AssetType.ToLower());

                xmlWriter.WriteStartElement("groups");

                var containerListPermissionsGrouped = containerList.Permissions.GroupBy(x => x.GroupName);
                foreach (var containerListPermissionGroup in containerListPermissionsGrouped)
                {
                    var groupName = containerListPermissionGroup.Key;
                    var permission = containerListPermissionGroup.First().Permission;

                    xmlWriter.WriteStartElement("group");
                    xmlWriter.WriteAttributeString("name", groupName);
                    xmlWriter.WriteAttributeString("permission", permission);
                    xmlWriter.WriteEndElement();

                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
        }
    }
}