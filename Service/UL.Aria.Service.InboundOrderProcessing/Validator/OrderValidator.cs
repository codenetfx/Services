using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.InboundOrderProcessing.Validator
{
    /// <summary>
    ///     Class OrderValidator.
    /// </summary>
    public sealed class OrderValidator : RequestValidator, IValidator
    {
        /// <summary>
        ///     Validates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        public new IDictionary<string, string> Validate(object dto)
        {
            //  • An inbound order message is invalid/incomplete if any of these validations fail:
            //  ◦ The customer is not populated
            //  ◦ There is no order type
            //  ◦ There is no order status
            //  ◦ There are no service line items (SalesOrderLine)
            //  ◦ There is no order number

            var orderDto = dto as IncomingOrderDto;
            base.Validate(orderDto);

            if (orderDto.CompanyId == null)
                Errors.Add(
                    string.Format("hasCompany(Customer External Id: {0})", orderDto.IncomingOrderCustomer.ExternalId),
                    "false");

            return Errors;
        }
    }
}