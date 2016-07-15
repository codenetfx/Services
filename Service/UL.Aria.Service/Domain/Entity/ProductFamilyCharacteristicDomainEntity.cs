using System;
using System.Collections.Generic;
using System.Linq;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ProductFamilyCharacteristicDomainEntity
    /// </summary>
    public abstract class ProductFamilyCharacteristicDomainEntity : TrackedDomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyCharacteristicDomainEntity" /> class.
        /// </summary>
        protected ProductFamilyCharacteristicDomainEntity() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyCharacteristicDomainEntity" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        protected ProductFamilyCharacteristicDomainEntity(Guid? id) : base(id)
        {
            Options = new List<ProductFamilyCharacteristicOption>();
        }

        /// <summary>
        ///     The value required
        /// </summary>
        public bool IsValueRequired { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The display name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the scope id.
        /// </summary>
        /// <value>The scope.</value>
        public Guid ScopeId { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic type id.
        /// </summary>
        /// <value>
        ///     The characteristic type id.
        /// </value>
        public Guid CharacteristicTypeId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the options.
        /// </summary>
        /// <value>
        ///     The options.
        /// </value>
        public IList<ProductFamilyCharacteristicOption> Options { get; set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public int SortOrder { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is multivalue.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is multivalue; otherwise, <c>false</c>.
        /// </value>
        public bool IsMultivalueAllowed
        {
            get
            {
                return GetBoolValue(ProductFamilyCharacteristicOptionName.AllowValueTypes,
                    ProductFamilyCharacteristicOptionValue.MultipleValue);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is range.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is range; otherwise, <c>false</c>.
        /// </value>
        public bool IsRangeAllowed
        {
            get
            {
                return GetBoolValue(ProductFamilyCharacteristicOptionName.AllowValueTypes,
                    ProductFamilyCharacteristicOptionValue.RangeValue);
            }
        }

        /// <summary>
        ///     Gets or sets the unit of measure id.
        /// </summary>
        /// <value>
        ///     The unit of measure id.
        /// </value>
        public Guid? UnitOfMeasureId { get; set; }

        private bool GetBoolValue(string name, string val)
        {
            return Options.Any(x => x.Name == name && x.Value == val);
        }

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return CoreEquals(obj);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return CoreGetHashCode();
		}
	}
}