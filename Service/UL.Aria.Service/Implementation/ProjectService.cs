using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     fulfills operations for working with <see cref="ProjectDto"/> objects.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProjectService : IProjectService
    {
		private const string ProjectExportFileNameFormatString = "Project Export - {0:MM}-{0:dd}-{0:yyyy}";

        private readonly IAuthorizationManager _authorizationManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProjectManager _projectManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectService" /> class.
        /// </summary>
        /// <param name="projectManager">The project manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public ProjectService(IProjectManager projectManager, IMapperRegistry mapperRegistry,
            IAuthorizationManager authorizationManager,
            IPrincipalResolver principalResolver
            )
        {
            _projectManager = projectManager;
            _mapperRegistry = mapperRegistry;
            _authorizationManager = authorizationManager;
            _principalResolver = principalResolver;
        }

        /// <summary>
        ///     Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     ProfileDto
        /// </returns>
        public ProjectDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var project = _projectManager.GetProjectById(new Guid(id)) ?? new Project();
            ValidateAccessRights(project);
            var projectDto = MapProjectToProjectDto(project);
            return projectDto;
        }

        /// <summary>
        ///     Gets the project download by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Stream GetProjectDownloadById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(id, "id");

            var context = WebOperationContext.Current;

            var projectDocument = _projectManager.GetProjectDownload(convertedId);
            if (null != context)
            {
                context.OutgoingResponse.Headers["Content-Disposition"] = "attachment; filename=" + id +
                                                                          ".xlsx";
                context.OutgoingResponse.ContentLength = projectDocument.Length;
                context.OutgoingResponse.ContentType =
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            return projectDocument;
        }

        /// <summary>
        ///     Gets the project download by id.
        /// </summary>
        /// <param name="ids">The ids , pipe delimited</param>
        /// <returns></returns>
        public Stream GetMultipleProjectDownload(string ids)
        {
            Guard.IsNotNullOrEmpty(ids, "id");
            var parsedIds = ids.Split('|');
            List<Guid> convertedIds = new List<Guid>();

            foreach (var id in parsedIds)
            {
                var convertedId = Guid.Parse(id);
                Guard.IsNotEmptyGuid(convertedId, "id");
                Guard.IsNotNull(ids, "id");
                convertedIds.Add(convertedId);
            }
           Guard.IsGreaterThan(0, convertedIds.Count, "ids");
            var context = WebOperationContext.Current;

            var projectDocument = _projectManager.GetMultipleProjectDownload(convertedIds);
            if (null != context)
            {
	            var fileName = string.Format(ProjectExportFileNameFormatString, DateTime.UtcNow);
				context.OutgoingResponse.Headers["Content-Disposition"] = string.Concat("attachment; filename=", fileName, ".xlsx");
                context.OutgoingResponse.ContentLength = projectDocument.Length;
                context.OutgoingResponse.ContentType =
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            return projectDocument;
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="project">The project.</param>
        public void Update(string id, ProjectDto project)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = Guid.Parse(id);
            Guard.IsNotEmptyGuid(idGuid, "id");
            Guard.IsNotNull(project, "project");

            var projectBo = MapProjectDtoToProject(project);
            _projectManager.Update(idGuid, projectBo);
        }

        internal ProjectDto MapProjectToProjectDto(Project project)
        {
            var serviceLines = project.ServiceLines;
            project.ServiceLines = null;
            var projectDto = _mapperRegistry.Map<ProjectDto>(project);
            project.ServiceLines = serviceLines;
            projectDto.ServiceLines = new List<IncomingOrderServiceLineDto>();

            foreach (var serviceLine in serviceLines)
                projectDto.ServiceLines.Add(_mapperRegistry.Map<IncomingOrderServiceLineDto>(serviceLine));

            return projectDto;
        }

        internal Project MapProjectDtoToProject(ProjectDto projectDto)
        {
            var serviceLines = projectDto.ServiceLines;
            projectDto.ServiceLines = null;
            var project = _mapperRegistry.Map<Project>(projectDto);
            projectDto.ServiceLines = serviceLines;
            project.ServiceLines = new List<IncomingOrderServiceLine>();

            foreach (var serviceLine in serviceLines)
                project.ServiceLines.Add(_mapperRegistry.Map<IncomingOrderServiceLine>(serviceLine));

            return project;
        }

        internal void ValidateAccessRights(Project project)
        {
            var claimsPrincipal = _principalResolver.Current;
            var values = string.Concat(project.ContainerId.GetValueOrDefault(Guid.Empty).ToString("N"), ",",
                project.CompanyId.GetValueOrDefault(Guid.Empty).ToString("N"));
            var resourceClaim = new System.Security.Claims.Claim(SecuredResources.ProjectInstace, values);
            var actionClaim = new System.Security.Claims.Claim(SecuredActions.View, values);
            var authorized = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);
            var hideFromCustomer = !claimsPrincipal.HasClaim(SecuredClaims.UlEmployee, SecuredActions.Role) && project.HideFromCustomer;

            if (!authorized ||hideFromCustomer)
                throw new UnauthorizedAccessException("You are not authorized to access this project");
        }

        /// <summary>
        /// Validates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
       /// <param name="project">The project.</param>
        /// <returns></returns>
        public IList<ProjectValidationEnumDto> Validate(string id,  ProjectDto project)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = Guid.Parse(id);
            Guard.IsNotEmptyGuid(idGuid, "id");
            //Guard.IsNotEmptyGuid(project.ContainerId, "ContainerId");

          return  _projectManager.Validate(id, MapProjectDtoToProject(project));
        }

    }
}