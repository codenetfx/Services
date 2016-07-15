using System.Collections.Generic;
using System.IO;
using System.Xml;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Parser
{
    /// <summary>
    ///     Class IncomingOrderParser
    /// </summary>
    [Entity(EntityTypeEnumDto.OrderDetail)]
    public sealed class OrderServiceLineDetailXmlParser : XmlParserBase, IXmlParser
    {
        private IList<OrderServiceLineDetailDto> OrderServiceLineDetails { get; set; }

        /// <summary>
        ///     Parses the specified incoming order message.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>NewProjectDto.</returns>
        public object Parse(string xml)
        {
            Guard.IsNotNullOrEmpty(xml, "OrderServiceLineDetailsMessage");

            OrderServiceLineDetails = new List<OrderServiceLineDetailDto>();
            var newOrderServiceLineDetail = new OrderServiceLineDetailDto {OriginalXml = xml};

            StringReader stringReader = null;

            try
            {
                stringReader = new StringReader(xml);

                using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings {IgnoreWhitespace = true}))
                {
                    stringReader = null;
                    ProcessRoot(newOrderServiceLineDetail, xmlReader, ProcessSubTreeRoot);
                }
            }
            finally
            {
                if (stringReader != null)
                    stringReader.Dispose();
            }

            return OrderServiceLineDetails;
        }

        private void ProcessSubTreeRoot(OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "DataArea":
                    ProcessSubTree(orderServiceLineDetail, xmlReader, ProcessSubTreeDataArea);
                    break;

                case "EBMHeader":
                    ProcessSubTree(orderServiceLineDetail, xmlReader, ProcessSubTreeEBMHeader);
                    break;
            }
        }

        private void ProcessSubTreeEBMHeader(OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Sender":
                    ProcessSubTree(orderServiceLineDetail, xmlReader, ProcessSubTreeEBMHeaderSender);
                    break;
            }
        }

        private void ProcessSubTreeEBMHeaderSender(OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    orderServiceLineDetail.SenderName = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeDataArea(OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ProcessWorkOrderResponse":
                    ProcessSubTree(orderServiceLineDetail, xmlReader, ProcessSubTreeDataAreaProcessWorkOrderResponse);
                    break;
            }
        }

        private void ProcessSubTreeDataAreaProcessWorkOrderResponse(OrderServiceLineDetailDto orderServiceLineDetail,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Identification":
                    ProcessSubTree(orderServiceLineDetail, xmlReader,
                        ProcessSubTreeDataAreaProcessWorkOrderResponseIdentification);
                    break;

                case "WorkOrderLine":
                    var xmlReaderSubTree = xmlReader.ReadSubtree();
                    xmlReaderSubTree.Read();
                    var orderServiceLineDetailXml = xmlReaderSubTree.ReadOuterXml();
                    var newOrderServiceLineDetail = new OrderServiceLineDetailDto
                    {
                        OrderNumber = orderServiceLineDetail.OrderNumber,
                        SenderName = orderServiceLineDetail.SenderName,
                        LineXml = orderServiceLineDetailXml,
                        OriginalXml = orderServiceLineDetail.OriginalXml,
                        HasCustom = false
                    };

                    StringReader stringReader = null;

                    try
                    {
                        stringReader = new StringReader(orderServiceLineDetailXml);

                        using (
                            var orderServiceLineDetailXmlReader = XmlReader.Create(stringReader,
                                new XmlReaderSettings {IgnoreWhitespace = true}))
                        {
                            stringReader = null;
                            orderServiceLineDetailXmlReader.Read();
                            ProcessSubTree(newOrderServiceLineDetail, orderServiceLineDetailXmlReader,
                                ProcessSubTreeDataAreaProcessWorkOrderResponseWorkOrderLine);
                        }
                    }
                    finally
                    {
                        if (stringReader != null)
                            stringReader.Dispose();
                    }

                    OrderServiceLineDetails.Add(newOrderServiceLineDetail);
                    break;
            }
        }

        private void ProcessSubTreeDataAreaProcessWorkOrderResponseIdentification(
            OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    orderServiceLineDetail.OrderNumber = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeDataAreaProcessWorkOrderResponseWorkOrderLine(
            OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Custom":
                    orderServiceLineDetail.HasCustom = true;
                    break;

                case "Identification":
                    ProcessSubTree(orderServiceLineDetail, xmlReader,
                        ProcessSubTreeDataAreaProcessWorkOrderResponseWorkOrderLineIdentification);
                    break;
            }
        }

        private void ProcessSubTreeDataAreaProcessWorkOrderResponseWorkOrderLineIdentification(
            OrderServiceLineDetailDto orderServiceLineDetail, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    orderServiceLineDetail.LineId = xmlReader.ReadString();
                    break;
            }
        }
    }
}