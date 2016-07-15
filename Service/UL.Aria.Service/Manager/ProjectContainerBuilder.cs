using System.Collections.Generic;
using System.Linq;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;

/// <summary>
/// Class to build a <see cref="Container"/> for a <see cref="Project"/>
/// </summary>
public class ProjectContainerBuilder:ContainerBuilderBase
{


    /// <summary>
    /// Builds the project.
    /// </summary>
    /// <param name="primarySearchEntityBase">The primary search entity base.</param>
    /// <param name="container">The container.</param>
    public static void BuildProject(PrimarySearchEntityBase primarySearchEntityBase, Container container)
    {
        new ProjectContainerBuilder().BuildContainer(primarySearchEntityBase, container);
    }
        

    /// <summary>
    /// Builds the container.
    /// </summary>
    /// <param name="primarySearchEntityBase">The primary search entity base.</param>
    /// <param name="container">The container.</param>
    protected override void BuildContainer(PrimarySearchEntityBase primarySearchEntityBase, Container container)
    {
        var containerId = container.Id.Value.ToString();
        var companyId = container.CompanyId.HasValue ? container.CompanyId.Value.ToString() : null;
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlAdministrator }, ContainerId = container.Id.Value, Value = "true" });
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlProjectAdministrator }, ContainerId = container.Id.Value, Value = "true" });
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.ContainerPrivate }, ContainerId = container.Id.Value, Value = containerId });
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.ContainerEdit }, ContainerId = container.Id.Value, Value = containerId });
        if (container.CompanyId.HasValue)
        {
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.CompanyAdmin }, ContainerId = container.Id.Value, Value = companyId });
            container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.CompanyProjectAccess }, ContainerId = container.Id.Value, Value = companyId });
        }
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlSystemAuditor }, ContainerId = container.Id.Value, Value = "true" });
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.ContainerView }, ContainerId = container.Id.Value, Value = containerId });
        container.AvailableClaims.Add(new ContainerAvailableClaim { Claim = new UL.Aria.Service.Domain.Entity.Claim { Uri = SecuredClaims.UlSystemOperations }, ContainerId = container.Id.Value, Value = "true" });

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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.UlProjectAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionContributor,
                        GroupName = ContainerDefinitionConstants.PrivateGroup
                    },
                    new ContainerListPermission
                    {
                        Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerPrivate, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionContributor,
                        GroupName = ContainerDefinitionConstants.PrivateGroup
                    }
                }
            });
        container.ContainerLists.Add(
            new ContainerList
            {
                AssetType = AssetTypeEnumDto.Document.ToString(),
                ContainerId = containerIdGuid,
                Name = "Modify",
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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.UlProjectAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerEdit, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionContributor,
                        GroupName = ContainerDefinitionConstants.ModifyGroup
                    },
                    new ContainerListPermission
                    {
                        Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerView, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionReader,
                        GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                    }
                }
            });

        if (container.CompanyId.HasValue)
        {
            var modifyList = container.ContainerLists.Last();
            modifyList.Permissions.Add(
                new ContainerListPermission
                {
                    Claim = new System.Security.Claims.Claim(SecuredClaims.CompanyAdmin, companyId, ContainerDefinitionConstants.DefaultClaimValueType),
                    Permission = ContainerDefinitionConstants.PermissionContributor,
                    GroupName = ContainerDefinitionConstants.ModifyGroup
                });
            modifyList.Permissions.Add(
                new ContainerListPermission
                {
                    Claim = new System.Security.Claims.Claim(SecuredClaims.CompanyProjectAccess, companyId, ContainerDefinitionConstants.DefaultClaimValueType),
                    Permission = ContainerDefinitionConstants.PermissionContributor,
                    GroupName = ContainerDefinitionConstants.ModifyGroup
                });
        }

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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.UlProjectAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerView, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerEdit, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionReader,
                        GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                    }
                }
            });



        if (container.CompanyId.HasValue)
        {
            var readOnlyList = container.ContainerLists.Last();
            readOnlyList.Permissions.Add(
                new ContainerListPermission
                {
                    Claim = new System.Security.Claims.Claim(SecuredClaims.CompanyAdmin, companyId, ContainerDefinitionConstants.DefaultClaimValueType),
                    Permission = ContainerDefinitionConstants.PermissionReader,
                    GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                });
            readOnlyList.Permissions.Add(
                new ContainerListPermission
                {
                    Claim = new System.Security.Claims.Claim(SecuredClaims.CompanyProjectAccess, companyId, ContainerDefinitionConstants.DefaultClaimValueType),
                    Permission = ContainerDefinitionConstants.PermissionReader,
                    GroupName = ContainerDefinitionConstants.ReadOnlyGroup
                });
        }

        container.ContainerLists.Add(
            new ContainerList
            {
                AssetType = "Task",
                ContainerId = containerIdGuid,
                Name = "Tasks",
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
                        Claim = new System.Security.Claims.Claim(SecuredClaims.UlProjectAdministrator, "true", ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionContributor,
                        GroupName = ContainerDefinitionConstants.PrivateGroup
                    },
                    new ContainerListPermission
                    {
                        Claim = new System.Security.Claims.Claim(SecuredClaims.ContainerPrivate, containerId, ContainerDefinitionConstants.DefaultClaimValueType),
                        Permission = ContainerDefinitionConstants.PermissionContributor,
                        GroupName = ContainerDefinitionConstants.PrivateGroup
                    }
                }
            });
    }
}