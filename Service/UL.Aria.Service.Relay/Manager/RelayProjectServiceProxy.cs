using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayProjectServiceProxy : ServiceAgentManagedProxy<IProjectService>, IProjectService
    {
        private WebChannelFactory<IProjectService> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProductServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public RelayProjectServiceProxy(IProxyConfigurationSource configurationSource) :
            this(
            new WebChannelFactory<IProjectService>(new WebHttpBinding(), configurationSource.ProjectService))
        {
        }

        
        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceAgentManagedProxy{T}" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private RelayProjectServiceProxy(WebChannelFactory<IProjectService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
            _factory = serviceProxyFactory;
        }

        /// <summary>
        /// Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ProfileDto
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProjectDto Fetch(string id)
        {
            ProjectDto product = null;
            IProjectService productService = ClientProxy ;
            using (new OperationContextScope((IContextChannel)productService))
            {
                product = productService.Fetch(id);
                return product;
            }
        }

        /// <summary>
        /// Gets the project download by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Stream GetProjectDownloadById(string id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="project">The project.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(string id, ProjectDto project)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the project download by id.
        /// </summary>
        /// <param name="ids">The ids, pipe delimited.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Stream GetMultipleProjectDownload(string ids)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Validates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ProjectValidationEnumDto> Validate(string id, ProjectDto project)
        {
            throw new NotSupportedException();
        }
    }
}
