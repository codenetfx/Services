using System;
using System.Collections.Generic;

namespace UL.Aria.Service.InboundOrderProcessing.Validator
{
    /// <summary>
    ///     Class ValidatorBase.
    /// </summary>
    public abstract class ValidatorBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidatorBase" /> class.
        /// </summary>
        protected ValidatorBase()
        {
            Errors = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        protected IDictionary<string, string> Errors { get; set; }
    }

    /// <summary>
    ///     Class StringUtility.
    /// </summary>
    public static class StringUtility
    {
        private static readonly char[] TrimChars = {'\r', '\n', '\t', ' '};

        /// <summary>
        ///     Determines whether the specified value has value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value has value; otherwise, <c>false</c>.</returns>
        public static bool HasValue(this string value)
        {
            return !String.IsNullOrEmpty((value ?? String.Empty).Trim(TrimChars));
        }
    }
}