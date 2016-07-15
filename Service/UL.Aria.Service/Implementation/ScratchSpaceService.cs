using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// a class that implemements the contract for Scratch storage space whereby temporary files can be stored
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    public class ScratchSpaceService : IScratchSpaceService
    {
        /// <summary>
        /// The _stratch space storage
        /// </summary>
        private readonly IScratchSpaceRepository _scratchSpaceRepository;
        /// <summary>
        /// The _mapper registry
        /// </summary>
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="scratchSpaceRepository">The stratch space storage.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public ScratchSpaceService(IScratchSpaceRepository scratchSpaceRepository, IMapperRegistry mapperRegistry)
        {
            _scratchSpaceRepository = scratchSpaceRepository;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ScratchFileDescriptorDto> FetchAll(string userId)
        {
            Guard.IsNotNullOrEmpty(userId, "userId");

            return _scratchSpaceRepository.FetchAll(Guid.Parse(userId)).Select(i => _mapperRegistry.Map<ScratchFileDescriptorDto>(i)).ToList();
        }

        /// <summary>
        /// Fetches the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Stream FetchContent(string userId, string fileId)
        {
            Guard.IsNotNullOrEmpty(userId, "userId");
            Guard.IsNotNullOrEmpty(fileId, "fileId");


            return _scratchSpaceRepository.FetchContent(Guid.Parse(userId), Guid.Parse(fileId));
        }

        /// <summary>
        /// Publishes the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileName"></param>
        /// <param name="contentStream">The content stream.</param>
        /// <returns>
        /// return the Id for that file
        /// </returns>
        public string PublishContent(string userId, string fileName, Stream contentStream)
        {
            Guard.IsNotNullOrEmpty(userId, "userId");

            return _scratchSpaceRepository.PublishContent(Guid.Parse(userId), fileName, contentStream).ToString();
        }

        /// <summary>
        /// Deletes the specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void Delete(string userId)
        {
            Guard.IsNotNullOrEmpty(userId, "userId");

            _scratchSpaceRepository.Purge(Guid.Parse(userId), true);
        }
    }
}