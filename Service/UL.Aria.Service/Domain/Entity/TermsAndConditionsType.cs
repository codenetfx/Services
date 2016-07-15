using System.ComponentModel;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// enum to specifiy the type of Terms and Coniditions as there will be different kinds
    /// </summary>
    public enum TermsAndConditionsType
    {
        /// <summary>
        /// Facilitates administrtaive access upon accepting Ts and Cs
        /// </summary>
        [Description("Facilitates administrative access upon accepting Terms and Conditions")]
        CompanyAdministrator,

        /// <summary>
        /// The none{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        [Description("No Terms and Conditions type has been specified. NULL state")]
        None
    }
}