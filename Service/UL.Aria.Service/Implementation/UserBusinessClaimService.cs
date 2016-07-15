using System;
using System.Collections.Generic;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Service for User Business Claims
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    public class UserBusinessClaimService : IUserBusinessClaimService
    {
        private readonly IUserBusinessClaimProvider _claimProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IUserBusinessClaimServiceValidator _userBusinessClaimServiceValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBusinessClaimService" /> class.
        /// </summary>
        /// <param name="claimProvider">The claim provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="userBusinessClaimServiceValidator">The user business claim service validator.</param>
        public UserBusinessClaimService(IUserBusinessClaimProvider claimProvider, IMapperRegistry mapperRegistry, IUserBusinessClaimServiceValidator userBusinessClaimServiceValidator)
        {
            _claimProvider = claimProvider;
            _mapperRegistry = mapperRegistry;
            _userBusinessClaimServiceValidator = userBusinessClaimServiceValidator;
        }


        /// <summary>
        /// Adds a user role..
        /// </summary>
        /// <param name="userClaimDto">The user role dto.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Add(UserBusinessClaimDto userClaimDto)
        {
            var mappedDto = _mapperRegistry.Map<UserBusinessClaim>(userClaimDto);

            _claimProvider.Add(mappedDto);
        }

        /// <summary>
        /// Removes the specified user claim.
        /// </summary>
        /// <param name="userClaimId"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Remove(string userClaimId)
        {
            _claimProvider.Remove(userClaimId);
        }

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserBusinessClaimDto> GetUserClaimValues(string claim, string loginId)
        {
            BusinessClaim mappedDto;
            string validatedLoginId;
            if (!_userBusinessClaimServiceValidator.TryParseSearchParameters(claim.DecodeFrom64(), loginId, out mappedDto, out validatedLoginId)) 
            {
                return new List<UserBusinessClaimDto>(); // TODO: needs error handling
            }

            return _mapperRegistry.Map<List<UserBusinessClaimDto>>(_claimProvider.GetUserClaimValues(mappedDto, validatedLoginId));
        }

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="userId">To string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserBusinessClaimHistoryDto> GetUserClaimHistory(string claim, string userId)
        {
            BusinessClaim mappedDto;
            string validatedLoginId;
            if (!_userBusinessClaimServiceValidator.TryParseSearchParameters(claim.DecodeFrom64(), userId, out mappedDto, out validatedLoginId))
            {
                return new List<UserBusinessClaimHistoryDto>(); // TODO: needs error handling
            }
            return _mapperRegistry.Map<List<UserBusinessClaimHistoryDto>>(_claimProvider.GetUserClaimHistory(mappedDto, validatedLoginId));
        }

        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserBusinessClaimDto> GetUserClaimsByValue(string claimValue)
        {
            return _mapperRegistry.Map<List<UserBusinessClaimDto>>(_claimProvider.GetUserClaimsByValue(claimValue));
        }

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserBusinessClaimDto> GetUserClaimsByClaimAndValue(string claim, string claimValue)
        {
            return _mapperRegistry.Map<List<UserBusinessClaimDto>>(_claimProvider.GetUserClaimsByClaimAndValue(claim.DecodeFrom64(), claimValue));
        }
    }
}