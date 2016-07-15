using System;
using System.Xml;
using UL.Aria.Common;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Parser
{
    /// <summary>
    ///     Class ParserBase
    /// </summary>
    public abstract class XmlParserBase
    {
        /// <summary>
        ///     Processes the sub tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto">The dto.</param>
        /// <param name="xmlReader">The XML reader.</param>
        /// <param name="processSubTreeProcessor">The process sub tree processor.</param>
        protected void ProcessSubTree<T>(T dto, XmlReader xmlReader, Action<T, XmlReader> processSubTreeProcessor)
        {
            var xmlReaderSubTree = xmlReader.ReadSubtree();
            xmlReaderSubTree.Read(); // reads value of current element
            xmlReaderSubTree.Read(); // Reads first child element

            while (!xmlReaderSubTree.EOF)
            {
                processSubTreeProcessor(dto, xmlReaderSubTree);
                xmlReaderSubTree.Skip();
            }
        }

        /// <summary>
        ///     Processes the root.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto">The dto.</param>
        /// <param name="xmlReader">The XML reader.</param>
        /// <param name="processRootProcessor">The process root processor.</param>
        protected void ProcessRoot<T>(T dto, XmlReader xmlReader, Action<T, XmlReader> processRootProcessor)
        {
            xmlReader.ReadStartElement();

            while (!xmlReader.EOF)
            {
                processRootProcessor(dto, xmlReader);
                xmlReader.Skip();
            }
        }
        
        /// <summary>
        /// Reads the int32.
        /// </summary>
        /// <param name="xmlReader">The XML reader.</param>
        /// <returns></returns>
        protected Int32 ReadInt32(XmlReader xmlReader)
        {
            var integer = xmlReader.ReadString();

            if (!string.IsNullOrEmpty(integer))
            {
                return Convert.ToInt32(integer);
            }

            return default(Int32);
        }

        /// <summary>
        /// Reads the date time.
        /// </summary>
        /// <param name="xmlReader">The XML reader.</param>
        /// <returns></returns>
        protected DateTime ReadDateTime(XmlReader xmlReader)
        {
            var dateTime = xmlReader.ReadString();

            if (!string.IsNullOrEmpty(dateTime))
            {
                return dateTime.ToUtc();
            }

            return default(DateTime);
        }

        /// <summary>
        /// Reads the date time nullable.
        /// </summary>
        /// <param name="xmlReader">The XML reader.</param>
        /// <returns></returns>
        protected DateTime? ReadDateTimeNullable(XmlReader xmlReader)
        {
            var dateTime = xmlReader.ReadString();

            if (!string.IsNullOrEmpty(dateTime))
            {
                return dateTime.ToUtc();
            }

            return null;
        }

    }
}