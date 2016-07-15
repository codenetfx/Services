using UL.Enterprise.Foundation.Data;
namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    ///     Class RefinementItem
    /// </summary>
    public class RefinementItem:IRefinementItem
    {
        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public long Count { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}