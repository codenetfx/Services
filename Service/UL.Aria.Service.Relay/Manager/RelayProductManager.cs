using System;
using System.Security.Claims;
using System.Threading;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Implements a <see cref="IRelayProductManager"/>
    /// </summary>
    public class RelayProductManager : IRelayProductManager
    {
        private readonly IProductService _productServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProductManager"/> class.
        /// </summary>
        /// <param name="productServiceProxy">The product service.</param>
        public RelayProductManager(IProductService productServiceProxy)
        {
            _productServiceProxy = productServiceProxy;
        }

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="productId">The product unique identifier.</param>
        /// <returns></returns>
        public ProductDto GetProductById(Guid productId)
        {
            Thread.CurrentPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                        {
                            new Claim(ClaimTypes.Role, "UL-Employee"),
                            new Claim(SecuredClaims.UlEmployee, SecuredActions.Role),
                            new Claim(ClaimTypes.Name, "pcd@nowhere.ul.com"), 
                            new Claim(SecuredClaims.UserId, "238c67d92aeae211804e54da2537410c"), 
                            new Claim("http://schema.ul.com/aria/admin","238c67d9-2aea-e211-804e-54da2537410c"),
                            new Claim("http://schema.ul.com/aria/employee", "46f65ea8-913d-4f36-9e28-89951e7ce8ef"),
                            new Claim("http://schema.ul.com/aria/admin/product", "46f65ea8-913d-4f36-9e28-89951e7ce8ef"),
                            new Claim("http://schema.ul.com/aria/company.companyaccess", "46f65ea8-913d-4f36-9e28-89951e7ce8ef")
                        }
                    ));
            return _productServiceProxy.GetProductById(productId.ToString("N"));
        }
    }
}