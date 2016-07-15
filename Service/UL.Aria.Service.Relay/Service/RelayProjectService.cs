using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Dto.Integration;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Relay.Domain;
using UL.Aria.Service.Relay.Manager;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Relay.Service
{
    /// <summary>
    ///     Implments <see cref="IRelayProjectService" /> to service project fetches.
    /// </summary>
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.PerCall,
        Namespace = @"http://aria.ul.com/Relay/ProjectDetail"
        )]
    public class RelayProjectService : IRelayProjectService
    {
        private const int MaximumSearchResults = 499;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IRelayCompanyManager _relayCompanyManager;
        private readonly IRelayProjectManager _relayProjectManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayProjectService" /> class.
        /// </summary>
        /// <param name="relayProjectManager">The relay project manager.</param>
        /// <param name="relayCompanyManager">The relay company manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public RelayProjectService(IRelayProjectManager relayProjectManager, IRelayCompanyManager relayCompanyManager,
            IMapperRegistry mapperRegistry)
        {
            _relayProjectManager = relayProjectManager;
            _relayCompanyManager = relayCompanyManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public FulfillmentProject GetProjectById(string id)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            var convertedId = ConvertGuid(id);
            var result = _relayProjectManager.GetProjectById(convertedId);
            if (null != result)
            {
                var projectMapped = _mapperRegistry.Map<FulfillmentProject>(result);
                var handler = _relayProjectManager.FetchProfile(projectMapped.ProjectHandler);
                if (null != handler)
                {
                    projectMapped.ProjectHandlerName = handler.DisplayName;
                }
                return projectMapped;
            }
            return null;
        }

        /// <summary>
        ///     Searches the projects.
        /// </summary>
        /// <param name="fulfillmentProjectSearchRequest"></param>
        /// <param name="includeDetail"></param>
        /// <returns></returns>
        public FulfillmentOrderProjectSearchResponse SearchProjects(FulfillmentOrderProjectSearchRequest fulfillmentProjectSearchRequest, bool includeDetail)
        {
           
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            //+ManagedProperty:Value for exact kql
            var searchCriteria = ConstructSearchCriteria(fulfillmentProjectSearchRequest);
            FulfillmentOrderProjectSearchResponse faultResult;
            if (ValidateCriteria(searchCriteria, out faultResult))
            {
                if (searchCriteria.Filters.Count == 0 && fulfillmentProjectSearchRequest.Filters.Any(x => x.FieldName == FulfillmentProjectSearchField.AccountNumber))
                    return new FulfillmentOrderProjectSearchResponse();
               
                var resultSet = _relayProjectManager.SearchProjects(searchCriteria);

                RelaySearchResultSet<TaskDto> completedTasksSet = null;
                RelaySearchResultSet<TaskDto> totalTasksSet = null;
                if (includeDetail)
                {
                    GetTaskData(resultSet, out completedTasksSet, out totalTasksSet);
                }

                if (null == resultSet)
                    return new FulfillmentOrderProjectSearchResponse();
                var result = ConstructResult(resultSet, completedTasksSet, totalTasksSet);

                return result;
            }
            return faultResult;
        }

        private void GetTaskData(RelaySearchResultSet<ProjectDto> resultSet, out RelaySearchResultSet<TaskDto> completedTasksSet, out RelaySearchResultSet<TaskDto> totalTasksSet)
        {
            if (null == resultSet || null == resultSet.Results)
            {
                completedTasksSet = new RelaySearchResultSet<TaskDto>();
                totalTasksSet = new RelaySearchResultSet<TaskDto>();
                return;
            }
            var ids = resultSet.Results.Select(x => x.ContainerId.ToString()).ToList();
            var taskCriteria = new SearchCriteriaDto
            {
                Filters = new Dictionary<string, List<string>>
                {
                    {AssetFieldNames.AriaContainerId, ids},
                },
                Keyword = "ariaTaskPhase=000400",
                EntityType = EntityTypeEnumDto.Task,
                Refiners = new List<string> {AssetFieldNames.AriaContainerId},
                StartIndex = 0,
                EndIndex = 1
            };
            completedTasksSet = _relayProjectManager.SearchProjectTaskSummary(taskCriteria);
            taskCriteria.Keyword = "";
            totalTasksSet = _relayProjectManager.SearchProjectTaskSummary(taskCriteria);
        }

        private SearchCriteriaDto ConstructSearchCriteria(FulfillmentOrderProjectSearchRequest fulfillmentProjectSearchRequest)
        {
            var searchCriteria = new SearchCriteriaDto
            {
                Filters = new Dictionary<string, List<string>>(),
                EntityType = EntityTypeEnumDto.Project,
                StartIndex = fulfillmentProjectSearchRequest.StartIndex ?? 0
            };
            searchCriteria.EndIndex = fulfillmentProjectSearchRequest.EndIndex ?? searchCriteria.StartIndex + MaximumSearchResults;
            searchCriteria.SortBy = AssetFieldNames.AriaProjectName;
            searchCriteria.SortDirection = SortDirectionDto.Ascending;
            searchCriteria.Sorts = new List<SortDto>
            {
                new SortDto {FieldName = AssetFieldNames.AriaProjectName, Order = SortDirectionDto.Ascending}
            };
            foreach (var fulfillmentOrderProjectSearchFilter in fulfillmentProjectSearchRequest.Filters)
            {
                SetFilter(FulfillmentProjectSearchField.OrderNumber, AssetFieldNames.AriaOrderNumber,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetFilter(FulfillmentProjectSearchField.ProjectStatus, AssetFieldNames.AriaProjectProjectStatus,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetFilter(FulfillmentProjectSearchField.PartySiteNumber, AssetFieldNames.AriaPartySiteNumber,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetFilter(FulfillmentProjectSearchField.QuoteNumber, AssetFieldNames.AriaQuoteNumber,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetFilter(FulfillmentProjectSearchField.ProductFileNumber, AssetFieldNames.AriaProjectFileNo,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetGuidFilter(FulfillmentProjectSearchField.ProjectId, AssetFieldNames.AriaProjectId,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetFilter(FulfillmentProjectSearchField.ProjectNumber, AssetFieldNames.AriaProjectNumber,
                    fulfillmentOrderProjectSearchFilter, searchCriteria);
                SetAccountNumberFilter(FulfillmentProjectSearchField.AccountNumber,
                    fulfillmentOrderProjectSearchFilter,
                    searchCriteria);

            }
            searchCriteria.Refiners = fulfillmentProjectSearchRequest.Refiners.Select(x =>
            {
                switch (x)
                {
                    case FulfillmentProjectRefinerField.OrderNumber:
                        return AssetFieldNames.AriaOrderNumber.ToString();
                    case FulfillmentProjectRefinerField.ProjectStatus:
                        return AssetFieldNames.AriaProjectProjectStatus.ToString();
                    default:
                        throw new ArgumentException("Refiners must be of type FulfillmentProjectRefinerField.");
                }

            }).ToList();
            return searchCriteria;
        }

        private FulfillmentOrderProjectSearchResponse ConstructResult(RelaySearchResultSet<ProjectDto> resultSet, RelaySearchResultSet<TaskDto> completedTasksSet, RelaySearchResultSet<TaskDto> totalTasksSet)
        {
            FulfillmentOrderProjectSearchResponse result = _mapperRegistry.Map<FulfillmentOrderProjectSearchResponse>(resultSet);
            
            var handlers = new Dictionary<string, ProfileDto>();
            var items = resultSet.Results;
            if (null != items)
            {
                var fulfillmentProjects = items.Select(x => _mapperRegistry.Map<FulfillmentProject>(x)).ToList();
                foreach (var fullfillmentProject in fulfillmentProjects)
                {
                    ProfileDto handler;
                    if (!handlers.TryGetValue(fullfillmentProject.ProjectHandler, out handler))
                    {
                        handler = _relayProjectManager.FetchProfile(fullfillmentProject.ProjectHandler);
                        if (null != handler)
                            handlers.Add(fullfillmentProject.ProjectHandler, handler);
                    }
                    if (null != handler)
                        fullfillmentProject.ProjectHandlerName = handler.DisplayName;
                    fullfillmentProject.CompletedTaskCount = GetTaskCount(completedTasksSet, fullfillmentProject);
                    fullfillmentProject.TaskCount = GetTaskCount(totalTasksSet, fullfillmentProject);
                }
                
                result.FulfillmentProjects = new FulfillmentProjects();
                result.FulfillmentProjects.AddRange(fulfillmentProjects);
            }
            return result;
        }

        private static long GetTaskCount(RelaySearchResultSet<TaskDto> relaySearchResultSet, FulfillmentProject fullfillmentProject)
        {
            long count = 0;
            if (relaySearchResultSet != null && relaySearchResultSet.RefinerResults != null)
            {
                if (relaySearchResultSet.RefinerResults.ContainsKey(AssetFieldNames.AriaContainerId))
                {
                    var refinementItemDto =
                        relaySearchResultSet.RefinerResults[AssetFieldNames.AriaContainerId].FirstOrDefault(
                            x => x.Value == fullfillmentProject.ContainerId.ToString());
                    if (refinementItemDto != null)
                    {
                        count = refinementItemDto.Count;
                    }
                }
            }
            return count;
        }

        private static bool ValidateCriteria(SearchCriteriaDto searchCriteria, out FulfillmentOrderProjectSearchResponse result)
        {
            if (MaximumSearchResults < searchCriteria.EndIndex - searchCriteria.StartIndex)
            {
                result = new FulfillmentOrderProjectSearchResponse();
                result.ErrorMessage =
                    "The total number of items requested would exceed the maximum allowed search results returned.";
                result.ErrorCode =
                    FulfillmentProjectMessageIds.RelayProjectServiceResultAskedForResultsExceedsMaximum;
                return false;
            }
            result = null;
            return true;
        }

        /// <summary>
        ///     Gets the project documents.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<FulfillmentDocumentMetaData> GetAllProjectDocuments(string projectId)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            var projectGuid = projectId.ToGuid();
            var results = _relayProjectManager.GetAllProjectDocuments(projectGuid);
            var handlers = new Dictionary<string, ProfileDto>();

            var fulfillmentDocumentMetaDatas = results.Results.Select(Map).ToList();
            FillLastModifiedByUser(projectId, fulfillmentDocumentMetaDatas, handlers);
            return fulfillmentDocumentMetaDatas;
        }

        /// <summary>
        ///     Pings this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Ping()
        {
            return _relayProjectManager.Ping();
        }

        private static Guid ConvertGuid(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            return convertedId;
        }

        /// <summary>
        ///     Sets the account number filter.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="fulfillmentProjectSearchCriteria">The fulfillment project search criteria.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>
        ///     <value>false</value>
        ///     if a matching company is not found,
        ///     <value>true</value>
        ///     if it is, or if the criteria did not contain an accout number.
        ///     If
        ///     <value>true</value>
        ///     , then it also sets the company id filter.
        /// </returns>
        internal bool SetAccountNumberFilter(FulfillmentProjectSearchField accountNumber,
            FulfillmentOrderProjectSearchFilter fulfillmentProjectSearchCriteria, SearchCriteriaDto searchCriteria)
        {
            var wereCompaniesFound = false;
            if (FulfillmentProjectSearchField.AccountNumber != fulfillmentProjectSearchCriteria.FieldName)
                return true;
            foreach (var searchValue in fulfillmentProjectSearchCriteria.SearchValues)
            {
                var company = _relayCompanyManager.FetchByExternalId(searchValue);
                if (null == company)
                    continue;
                searchCriteria.Filters.Add(AssetFieldNames.AriaCompanyId,
                    new List<string> {company.Id.Value.ToString().ToLowerInvariant()});
                wereCompaniesFound = true;
            }
            return wereCompaniesFound;
        }

        private void FillLastModifiedByUser(string projectId,
            IEnumerable<FulfillmentDocumentMetaData> fulfillmentDocumentMetaDatas,
            Dictionary<string, ProfileDto> handlers)
        {
            foreach (var result in fulfillmentDocumentMetaDatas)
            {
                var handlerId = result.LastModifiedBy;
                if (string.IsNullOrWhiteSpace(handlerId))
                    continue;
                ProfileDto profile;
                if (!handlers.TryGetValue(handlerId, out profile))
                {
                    profile = _relayProjectManager.FetchProfile(handlerId);

                    if (null != projectId)
                        handlers.Add(handlerId, profile);
                }
                if (null != profile)
                {
                    result.LastModifiedByName = profile.DisplayName;
                }
            }
        }

        private static void SetFilter(FulfillmentProjectSearchField fieldName, string assetFieldName,
            FulfillmentOrderProjectSearchFilter fulfillmentProjectSearchCriteria, SearchCriteriaDto searchCriteria)
        {
            if (fulfillmentProjectSearchCriteria.FieldName == fieldName)
            {
                List<string> values;
                if (!searchCriteria.Filters.TryGetValue(assetFieldName, out values))
                {
                    values = new List<string>();
                    searchCriteria.Filters.Add(assetFieldName, values);
                }
                values.AddRange(fulfillmentProjectSearchCriteria.SearchValues);
            }
        }

        private static void SetGuidFilter(FulfillmentProjectSearchField fieldName, string assetFieldName,
            FulfillmentOrderProjectSearchFilter fulfillmentProjectSearchFilter, SearchCriteriaDto searchCriteria)
        {
            var newFilter = new FulfillmentOrderProjectSearchFilter
            {
                FieldName = fulfillmentProjectSearchFilter.FieldName
            };
            if (fulfillmentProjectSearchFilter.FieldName == fieldName)
            {
                newFilter.SearchValues = new SearchValues();
                newFilter.SearchValues.AddRange(
                    fulfillmentProjectSearchFilter.SearchValues.Select(
                    x =>
                    {
                        Guid guid;
                        return Guid.TryParse(x, out guid)
                            ? guid.ToString()
                            : x;
                    }));
                SetFilter(fieldName, assetFieldName, newFilter, searchCriteria);
            }
        }

        private FulfillmentDocumentMetaData Map(SearchResultDto searchResultDto)
        {
            var value = searchResultDto.Metadata;

            var document = new FulfillmentDocumentMetaData
            {
                DocumentId = value.GetValue(AssetFieldNames.AriaDocumentId, default(string)),
                ContentType = value.GetValue(AssetFieldNames.AriaContentType, (string) null),
                DocumentPermission = value.GetValue(AssetFieldNames.AriaPermission, "None"),
                FileSize = value.GetValue(AssetFieldNames.AriaSize, 0),
                DocumentFileName = value.GetValue(AssetFieldNames.AriaName, string.Empty),
                DocumentTitle = value.GetValue(AssetFieldNames.AriaTitle, string.Empty),
                LastModifiedBy = value.GetValue(AssetFieldNames.AriaLastModifiedBy, string.Empty),
                LastModifiedDate = value.GetValue(AssetFieldNames.AriaLastModifiedOn, default(DateTime))
            };
            var documentType = value.GetValue(AssetFieldNames.AriaDocumentTypeId, default(Guid));
            if (documentType != default(Guid))
            {
                var documentTypeDto = DocumentTypeDto.DocumentTypes.FirstOrDefault(x => x.Id == documentType);
                document.DocumentType = documentTypeDto != null ? documentTypeDto.Name : null;
            }
            return document;
        }
    }
}