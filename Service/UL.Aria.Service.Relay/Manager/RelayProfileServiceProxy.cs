using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Relay for <see cref="IProfileService"/>
    /// </summary>
    public class RelayProfileServiceProxy : ServiceAgentManagedProxy<ISimpleProfileService>, ISimpleProfileService
    {
        private WebChannelFactory<ISimpleProfileService> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProductServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public RelayProfileServiceProxy(IProxyConfigurationSource configurationSource) :
            this(
            new WebChannelFactory<ISimpleProfileService>(new WebHttpBinding(), configurationSource.ProfileService))
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceAgentManagedProxy{T}" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private RelayProfileServiceProxy(WebChannelFactory<ISimpleProfileService> serviceProxyFactory)
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
        /// <exception cref="System.NotSupportedException"></exception>
        public ProfileDto FetchByIdOrUserName(string id)
        {
            ISimpleProfileService profileService = ClientProxy;
            using (new OperationContextScope((IContextChannel)profileService))
            {
                return FetchByIdOrUserNameImpl(id, profileService);
            }
        }

        internal static ProfileDto FetchByIdOrUserNameImpl(string id, ISimpleProfileService profileService)
        {
            ProfileDto profileDto;
            try
            {
                profileDto = profileService.FetchByIdOrUserName(id);
                return profileDto;
            }
            catch (EndpointNotFoundException)
            {
                return null;
            }
        }
    }
}