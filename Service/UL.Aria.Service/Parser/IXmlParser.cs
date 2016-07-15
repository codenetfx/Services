namespace UL.Aria.Service.Parser
{
    /// <summary>
    ///     Interface IIncomingOrderParser
    /// </summary>
    public interface IXmlParser
    {
        /// <summary>
        ///     Parses the specified incoming order message.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>NewProjectDto.</returns>
        object Parse(string xml);
    }
}