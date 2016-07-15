namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// ProductFamilyCharacteristicUpload
    /// </summary>
    public class ProductFamilyCharacteristicUpload
    {
        /// <summary>
        /// Gets or sets the upload action.
        /// </summary>
        /// <value>
        /// The upload action.
        /// </value>
        public UploadAction UploadAction { get; set; }
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public ProductFamilyCharacteristicDomainEntity Entity { get; set; }
        

    }
}