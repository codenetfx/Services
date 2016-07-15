using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Relay.Domain;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Implements operations for <see cref="IRelayProjectManager"/>
    /// </summary>
   
    public class RelayProjectManager : IRelayProjectManager
    {
        private readonly IProjectService _projectService;
        private readonly IAriaService _ariaService;
        private readonly ISimpleProfileService _profileService;
        private readonly IRelayCompanyManager _relayCompanyManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProjectManager" /> class.
        /// </summary>
        /// <param name="projectService">The project service.</param>
        /// <param name="ariaService">The aria service.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="relayCompanyManager">The company service.</param>
        public RelayProjectManager(IProjectService projectService, IAriaService ariaService, ISimpleProfileService profileService, IRelayCompanyManager relayCompanyManager)
        {
            _projectService = projectService;
            _ariaService = ariaService;
            _profileService = profileService;
            _relayCompanyManager = relayCompanyManager;
        }

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProjectDto GetProjectById(Guid id)
        {
            //SetClaims();
            try
            {
                var project = _projectService.Fetch(id.ToString());
                FillCompany(project);
                return project;
            }
            catch (EndpointNotFoundException)
            {
                return null;
            }
            
        }

        private void FillCompany(ProjectDto project)
        {
            if (null == project)
                return;
            if (string.IsNullOrWhiteSpace(project.CompanyName) && project.CompanyId.HasValue)
            {
                var companyDto = _relayCompanyManager.FetchById(project.CompanyId.Value);
                if (null != companyDto)
                    project.CompanyName = companyDto.Name;
            }
        }

        /// <summary>
        /// Fetches the project handler.
        /// </summary>
        /// <param name="projectHandler">The project handler.</param>
        /// <returns></returns>
        public ProfileDto FetchProfile(string projectHandler)
        {
            return _profileService.FetchByIdOrUserName(projectHandler);
        }

        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public RelaySearchResultSet<ProjectDto> SearchProjects(SearchCriteriaDto searchCriteria)
        {
           // SetClaims();
            var searchResultSetDto = _ariaService.Search(searchCriteria);
            if (null == searchResultSetDto || null == searchResultSetDto.Results)
                return new RelaySearchResultSet<ProjectDto>();
            var projects = searchResultSetDto.Results.Select(x => MapSearchResult(x.Metadata)).ToList();
            return new RelaySearchResultSet<ProjectDto>{Results = projects, RefinerResults = searchResultSetDto.RefinerResults, Summary = searchResultSetDto.Summary};
        }

        /// <summary>
        /// Searches the project task summary.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public RelaySearchResultSet<TaskDto> SearchProjectTaskSummary(SearchCriteriaDto searchCriteria)
        {
           // SetClaims();
            var searchResultSetDto = _ariaService.Search(searchCriteria);
            if (null == searchResultSetDto || null == searchResultSetDto.Results)
                return new RelaySearchResultSet<TaskDto>();
            return new RelaySearchResultSet<TaskDto>{RefinerResults = searchResultSetDto.RefinerResults, Summary = searchResultSetDto.Summary};
        }

        /// <summary>
        /// Fetches all documents.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public SearchResultSetDto GetAllProjectDocuments(Guid projectId)
        {
            var searchCriteria = new SearchCriteriaDto
            {
	            EntityType = EntityTypeEnumDto.Project,
	            Filters =
		            new Dictionary<string, List<string>>
		            {
			            {AssetFieldNames.AriaProjectId, new List<string> {projectId.ToString()}}
		            }
            };

	        var containerSearch = _ariaService.Search(searchCriteria);
            var containerId = containerSearch.Results.Single().Metadata[AssetFieldNames.AriaContainerId].ToGuid();

            return _ariaService.FetchAllDocuments(containerId.ToString());
        }


        private static void SetClaims()
        {
            Thread.CurrentPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Role, "UL-Employee"),
                        new Claim(SecuredClaims.UlEmployee, SecuredActions.Role),
                        new Claim(ClaimTypes.Name, "pcd@nowhere.ul.com"),
                        new Claim(SecuredClaims.UserId, "238c67d92aeae211804e54da2537410c"),
                        new Claim("http://schema.ul.com/aria/employee", "46f65ea8-913d-4f36-9e28-89951e7ce8ef"),
                        new Claim("http://schema.ul.com/aria/admin/project", "true"),
                        new Claim("http://schema.ul.com/aria/admin/product", "true"),
                        new Claim("http://schema.ul.com/aria/admin/order", "true"),
                        new Claim("http://schema.ul.com/aria/admin", "true"),
                        new Claim("http://schema.ul.com/aria/company.companyaccess", "46f65ea8-913d-4f36-9e28-89951e7ce8ef")
                    }
                    ));
        }

        private ProjectDto MapSearchResult(IDictionary<string, string> value)
        {
            var projectDto = new ProjectDto();
            string projectIdList;
            string[] projectIds;
            if (value.TryGetValue(AssetFieldNames.AriaProjectId, out projectIdList) &&
                (projectIds = projectIdList.Split(AssetFieldNames.SharePointMultivalueSeparator)).Length > 0)
            {
                projectDto.Id = Guid.Parse(projectIds[0]);
            }

            projectDto.ContainerId = value.GetValue(AssetFieldNames.AriaContainerId, projectDto.ContainerId);
            projectDto.ProjectHandler = value.GetValue( AssetFieldNames.AriaProjectHandler, projectDto.ProjectHandler);
            projectDto.CompanyName = value.GetValue( AssetFieldNames.AriaCompanyName, projectDto.CompanyName);
            projectDto.ProjectName = value.GetValue(AssetFieldNames.AriaCustomerProjectName, projectDto.ProjectName);
            projectDto.Name = value.GetValue(AssetFieldNames.AriaProjectName, projectDto.Name);
            projectDto.ProjectStatus = value.GetValue(AssetFieldNames.AriaProjectProjectStatus, projectDto.ProjectStatus);
            projectDto.CustomerRequestedDate = GetDate(value, AssetFieldNames.AriaProjectDueDate, projectDto.CustomerRequestedDate);
            projectDto.EndDate = GetDate(value, AssetFieldNames.AriaProjectEndDate, projectDto.EndDate);
            projectDto.CompanyId = value.GetValue(AssetFieldNames.AriaCompanyId, projectDto.CompanyId);
            projectDto.OrderNumber = value.GetValue(AssetFieldNames.AriaOrderNumber, projectDto.OrderNumber);
            projectDto.Expedited = value.GetValue(AssetFieldNames.AriaProjectExpedited, projectDto.Expedited);
            projectDto.DateBooked = GetDate(value, AssetFieldNames.AriaDateBooked, projectDto.DateBooked);
            projectDto.Status = value.GetValue(AssetFieldNames.AriaProjectStatus, projectDto.Status);
            projectDto.FileNo = value.GetValue(AssetFieldNames.AriaProjectFileNo, projectDto.FileNo);
            projectDto.CCN = value.GetValue(AssetFieldNames.AriaProjectCcn, projectDto.CCN);
            
            var project = GetProjectById(projectDto.Id.Value);
            if (null != project)
            {
                projectDto.ServiceLines = project.ServiceLines;
                projectDto.ShipToContact = project.ShipToContact;
                projectDto.EndDate = project.EndDate;
                projectDto.CCN = project.CCN;
                projectDto.FileNo = project.FileNo;
                projectDto.QuoteNo = project.QuoteNo;
                projectDto.ProjectNumber = project.ProjectNumber;
                projectDto.IncomingOrderCustomer = project.IncomingOrderCustomer;
            }
           
            return projectDto;
        }
       
        /// <summary>
		/// Parses the date and returns a date in UTC format.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		internal static DateTime? GetDate(IDictionary<string, string> metadata, string fieldName, DateTime? defaultValue)
		{
			string value;
			if (!metadata.TryGetValue(fieldName, out value))
			{
				return defaultValue;
			}

			//
			// DateTimeStyles.AdjustToUniversal is the key part here, when .NET sees a time zone specifier in the 
			// string (which is what SharePoint does) then it will convert -- incorrectly -- to local time (ie this machine's time)
			// we want to keep it as UTC and, correctly, convert to the *user's* date time only when we display it
			//
			DateTime date;
			if (!DateTime.TryParse(value, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AdjustToUniversal, out date))
			{
				return defaultValue;
			}

			return date;
		}

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Ping()
		{
			_profileService.FetchByIdOrUserName("a");
			_ariaService.Search(new SearchCriteriaDto{Keyword = "a"});
			return true;
		}
	}
}