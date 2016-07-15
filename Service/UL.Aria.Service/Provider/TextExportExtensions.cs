using System;
using System.IO;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Static class with helpers for writing to text for export.
    /// </summary>
    public static class TextExportExtensions
    {

        /// <summary>
        /// Writes the value.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        public static void WriteValue(this StreamWriter writer, object value)
        {
            writer.Write("{0}\t", value == null ? string.Empty : value.ToString().Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\t", " "));
        }
    }
}