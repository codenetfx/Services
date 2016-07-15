using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Results;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;


namespace UL.Aria.Service.Update.Managers
{

    /// <summary>
    /// Provides an update manager for updating just the project's search meta info in bulk
    /// </summary>
    [Entity(EntityTypeEnumDto.ProjectMeta)]
    public class ProjectMetaOnlyUpdateManager : ProjectUpdateManager, IUpdateManager
    {
        private const EntityTypeEnumDto EntityType = EntityTypeEnumDto.Project;
        private readonly IProjectProvider _projectProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly Parser.IXmlParser _parser;
        private readonly IAssetProvider _assetProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectUpdateManager" /> class.
        /// </summary>
        /// <param name="projectProvider">The project provider.</param>
        /// <param name="parserResolver">The parser resolver.</param>
        /// <param name="fileLogger">The file logger.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="assetProvider">The asset provider.</param>
        public ProjectMetaOnlyUpdateManager(IProjectProvider projectProvider, IXmlParserResolver parserResolver,
            IFileLogger fileLogger, IUpdateConfigurationSource config, IMapperRegistry mapperRegistry, IAssetProvider assetProvider)
            : base(projectProvider, parserResolver, fileLogger, config, mapperRegistry)
        {
            this._projectProvider = projectProvider;
            _mapperRegistry = mapperRegistry;
            _parser = parserResolver.Resolve(EntityType.ToString());
            _assetProvider = assetProvider;
        }


        /// <summary>
        /// When implemented in a derived class it gets the Item Processing function.
        /// </summary>
        /// <param name="lookup">The lookup.</param>
        /// <returns></returns>
        internal protected override Func<TaskExecutionResult> GetItemProcessingFunction(Lookup lookup)
        {
            Func<TaskExecutionResult> func = () =>
            {
                try
                {
                    var project = _projectProvider.FetchById(lookup.Id.Value);
                    if (project != null && project.Id.GetValueOrDefault() != Guid.Empty)
                    {
                        _assetProvider.Update(project.Id.Value, project);
                        return new TaskExecutionResult(lookup, true);
                    }
                    else
                    {
                        return new TaskExecutionResult(lookup, false, "Project not found.");
                    }                    
                }
                catch (Exception ex)
                {
                    return new TaskExecutionResult(lookup, false, ex.Message);
                }

            };

            return func;
        }
    }
}
