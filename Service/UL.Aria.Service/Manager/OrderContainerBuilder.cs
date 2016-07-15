using System.Collections.Generic;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Class to build <see cref="Container"/> objects from <see cref="Order"/> objects
    /// </summary>
    public class OrderContainerBuilder: ContainerBuilderBase
    {

        /// <summary>
        /// Builds the project.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="container">The container.</param>
        public static void BuildOrder(PrimarySearchEntityBase primarySearchEntityBase, Container container)
        {
            new OrderContainerBuilder().BuildContainer(primarySearchEntityBase, container);
        }

        /// <summary>
        /// Builds the container.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="container">The container.</param>
        protected override void BuildContainer(PrimarySearchEntityBase primarySearchEntityBase, Container container)
        {
            Guard.IsNotNull(primarySearchEntityBase.CompanyId, "companyId");
            var containerId = container.Id.Value.ToString();
            var companyId = container.CompanyId.ToString();
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlAdministrator }, ContainerId = container.Id.Value, Value = "true" });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlOrderAdministrator }, ContainerId = container.Id.Value, Value = "true" });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlSystemAuditor }, ContainerId = container.Id.Value, Value = "true" });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlSystemOperations }, ContainerId = container.Id.Value, Value = "true" });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.CompanyAdmin }, ContainerId = container.Id.Value, Value = companyId });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.CompanyOrderAccess }, ContainerId = container.Id.Value, Value = companyId });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.ContainerView }, ContainerId = container.Id.Value, Value = containerId });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.ContainerPrivate }, ContainerId = container.Id.Value, Value = containerId });

            var containerIdGuid = container.Id.Value;
            container.ContainerLists.Add(
                new ContainerList
                {
                    AssetType = AssetTypeEnumDto.Document.ToString(),
                    ContainerId = containerIdGuid, 
                    Name = "Private", 
                    Permissions = new List<ContainerListPermission>
                    {
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.UlAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionContributor,
                            GroupName = ContainerDefinitionConstants.PrivateGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.UlOrderAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionContributor,
                            GroupName = ContainerDefinitionConstants.PrivateGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerPrivate, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionContributor,
                            GroupName = ContainerDefinitionConstants.PrivateGroup
                        }
                    },
                });
            container.ContainerLists.Add(
                new ContainerList
                {
                    AssetType = AssetTypeEnumDto.Document.ToString(),
                    ContainerId = containerIdGuid,
                    Name = "ReadOnly",
                    Permissions = new List<ContainerListPermission>
                    {
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.UlAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionContributor,
                            GroupName = ContainerDefinitionConstants.PrivateGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.UlOrderAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionContributor,
                            GroupName = ContainerDefinitionConstants.PrivateGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerPrivate, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionContributor,
                            GroupName = ContainerDefinitionConstants.PrivateGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.UlSystemAuditor, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionReader,
                            GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.UlSystemOperations, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionReader,
                            GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.CompanyAdmin, companyId, ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionReader,
                            GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.CompanyOrderAccess, companyId, ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionReader,
                            GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                        },
                        new ContainerListPermission
                        {
                            Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerView, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                            Permission = ContainerDefinitionConstants.PermissionReader,
                            GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                        }
                    }
                });
        }
    }
}