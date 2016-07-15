using System.Collections.Generic;
using System.Linq;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.InboundOrderProcessing.Validator
{
    /// <summary>
    ///     Class RequestValidator.
    /// </summary>
    public class RequestValidator : ValidatorBase, IValidator
    {
        /// <summary>
        ///     Validates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        public IDictionary<string, string> Validate(object dto)
        {
            //  • An inbound order message is invalid/incomplete if any of these validations fail:
            //  ◦ The customer is not populated
            //  ◦ There is no order type
            //  ◦ There is no order status
            //  ◦ There are no service line items (SalesOrderLine)
            //  ◦ There is no order number

            var requestDto = dto as IncomingOrderDto;
            Guard.IsNotNull(requestDto, "orderDto");

            var hasCustomer = requestDto.IncomingOrderCustomer != null;
            if (!hasCustomer)
                Errors.Add("hasCustomer", "false");

            if (!requestDto.OrderNumber.HasValue())
                Errors.Add("hasOrderNumber", "false");

            if (!requestDto.OrderType.HasValue())
                Errors.Add("hasOrderType", "false");

            if (!requestDto.Status.HasValue())
                Errors.Add("hasOrderStatus", "false");

            var hasServiceLines = requestDto.ServiceLines != null && requestDto.ServiceLines.Any();
            if (!hasServiceLines)
                Errors.Add("hasServiceLines", "false");

            return Errors;
        }
    }
}