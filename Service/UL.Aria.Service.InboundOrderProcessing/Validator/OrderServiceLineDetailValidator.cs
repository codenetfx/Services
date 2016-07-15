using System.Collections.Generic;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.InboundOrderProcessing.Validator
{
    /// <summary>
    ///     Class OrderServiceLineDetailValidator. This class cannot be inherited.
    /// </summary>
    public sealed class OrderServiceLineDetailValidator : ValidatorBase, IValidator
    {
        /// <summary>
        ///     Validates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        public IDictionary<string, string> Validate(object dto)
        {
            var orderServiceLineDetailDto = dto as OrderServiceLineDetailDto;
            Guard.IsNotNull(orderServiceLineDetailDto, "orderServiceLineDetailDto");

// ReSharper disable once PossibleNullReferenceException
            if (!orderServiceLineDetailDto.OrderId.HasValue)
                Errors.Add(
                    string.Format("hasOrder(Order Number: {0})", orderServiceLineDetailDto.OrderNumber), "false");

            if (!orderServiceLineDetailDto.LineId.HasValue())
                Errors.Add("hasLineId", "false");

            if (!orderServiceLineDetailDto.SenderName.HasValue())
                Errors.Add("hasSenderName", "false");

            if (!orderServiceLineDetailDto.SenderId.HasValue)
                Errors.Add(string.Format("hasSenderId, Sender Name: {0}", orderServiceLineDetailDto.SenderName ?? ""), "false");

            if (!orderServiceLineDetailDto.HasCustom)
                Errors.Add("hasCustom", "false");

            return Errors;
        }
    }
}