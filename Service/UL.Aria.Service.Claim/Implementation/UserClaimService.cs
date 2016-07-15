using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Domain;
using UL.Aria.Service.Claim.Provider;

namespace UL.Aria.Service.Claim.Implementation
{
    /// <summary>
    /// User claim service class.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserClaimService : IUserClaimService
    {
        private readonly IUserClaimProvider _userClaimProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimService" /> class.
        /// </summary>
        /// <param name="userClaimProvider">The user claim manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public UserClaimService(IUserClaimProvider userClaimProvider, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _userClaimProvider = userClaimProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Adds a user role.
        /// </summary>
        /// <param name="userClaimDto">The user role dto.</param>
        [FaultContract(typeof(InvalidOperationException))]
        public void Add(UserClaimDto userClaimDto)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                var userClaim = _mapperRegistry.Map<UserClaim>(userClaimDto);
                _userClaimProvider.Add(userClaim);

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Removes the specified user claim.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        public void Remove(string userClaimId)
        {
            Guid userClaimGuid = userClaimId.ToGuid();

            using (var transactionScope = _transactionFactory.Create())
            {
                _userClaimProvider.Remove(userClaimGuid);

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id (base 64 encoded).</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserClaimDto> GetUserClaimValues(string claimId, string loginId)
        {
            var claimUri = new Uri(claimId.DecodeFrom64());

            using (var transaction = _transactionFactory.Create())
            {
                //Need to encode the claim id as a base 64 value since IIS blocks the URL encoded value
                var result = _mapperRegistry.Map<List<UserClaimDto>>(_userClaimProvider.GetUserClaimValues(claimUri, loginId));

                transaction.Complete();

                return result;
            }
        }

        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaimDto> GetUserClaimsByValue(string claimValue)
        {
            using (var transaction = _transactionFactory.Create())
            {
                //Need to encode the claim id as a base 64 value since IIS blocks the URL encoded value
                var result = _mapperRegistry.Map<List<UserClaimDto>>(_userClaimProvider.GetUserClaimsByValue(claimValue));

                transaction.Complete();

                return result;
            }
        } 

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaimDto> GetUserClaimsByIdAndValue(string claimId, string claimValue)
        {
            var claimUri = new Uri(claimId.DecodeFrom64());
            using (var transaction = _transactionFactory.Create())
            {
                //Need to encode the claim id as a base 64 value since IIS blocks the URL encoded value
                var result = _mapperRegistry.Map<List<UserClaimDto>>(_userClaimProvider.GetUserClaimsByIdAndValue(claimUri.ToString(), claimValue));

                transaction.Complete();

                return result;
            }
        }

        /// <summary>
        /// Gets all of the claim values for a user.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserClaimDto> GetUserClaims(string loginId)
        {
            

            using (var transaction = _transactionFactory.Create())
            {
                //Need to encode the claim id as a base 64 value since IIS blocks the URL encoded value
                IList<UserClaim> userClaimValues = _userClaimProvider.GetUserClaimValues(loginId);
                var result = userClaimValues.Select(_mapperRegistry.Map<UserClaimDto>).ToList();

                transaction.Complete();

                return result;
            }
        }

        /// <summary>
        /// Removes claims for a user.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <returns></returns>
        public void RemoveUserClaims(string loginId)
        {

            using (var transaction = _transactionFactory.Create())
            {
                _userClaimProvider.RemoveUserClaims(loginId);

                transaction.Complete();

                
            }
        }

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claimId">The encode to64.</param>
        /// <param name="loginId">The login id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserClaimHistoryDto> GetUserClaimHistory(string claimId, string loginId)
        {
            var claimUri = new Uri(claimId.DecodeFrom64());
            
            using (var transaction = _transactionFactory.Create())
            {
                var results = _mapperRegistry.Map<List<UserClaimHistoryDto>>(_userClaimProvider.GetUserClaimHistory(claimUri, loginId));

                transaction.Complete();

                return results;
            }
        }
    }
}