using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Security.AntiXss;
using System.Web.UI.WebControls.Expressions;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations that help manage grouping <see cref="ProductFamilyCharacteristicDomainEntity" /> objects. 
    /// </summary>
    public class ProductFamilyCharacteristicGroupHelper : IProductFamilyCharacteristicGroupHelper
    {
        private static readonly Guid BaseScopeId = new Guid("4CAA5706-6D86-E211-BCF5-20C9D042ED3E");

        private static readonly Guid descriptiveId = new Guid("FC08A508-7FBD-E211-832C-54D9DFE94C0D");
        private static readonly Guid operationalId = new Guid("14128A5F-6D86-E211-BCF5-20C9D042ED3E");
        private static readonly Guid constructionId = new Guid("13128A5F-6D86-E211-BCF5-20C9D042ED3E");

        /// <summary>
        /// Groups the characteristics.
        /// </summary>
        /// <param name="characteristics">The characteristics.</param>
        /// <param name="baseCharacteristics">The base characteristics.</param>
        /// <param name="variableCharacteristics">The variable characteristics.</param>
        public void GroupCharacteristics(IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, out IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics,
                                         out IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics)
        {
            baseCharacteristics = characteristics;
            
            variableCharacteristics = new List<ProductFamilyCharacteristicDomainEntity>();// characteristics.Where(x => x.ScopeId != BaseScopeId);
        }

    }
}