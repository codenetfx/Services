using System;
using System.Globalization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class TaskProgressEnumDtoExtensions
    /// </summary>
    public static class TaskProgressEnumDtoExtensions
    {
        /// <summary>
        ///     To the share point string.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>System.String.</returns>
        public static string ToSharePointString(this TaskProgressEnumDto val)
        {
            return ((int)val).ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');
        }

        /// <summary>
        ///     To the task progress enum dto.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>UL.Aria.Service.Contracts.Dto.TaskProgressEnumDto.</returns>
        public static TaskProgressEnumDto FromSharePointStringToTaskProgressEnumDto(this string val)
        {
            return (TaskProgressEnumDto)Convert.ToInt32(val);
        }
		
        /// <summary>
        ///     To the task progress enum dto.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>TaskProgressEnumDto.</returns>
        public static TaskProgressEnumDto ToTaskProgressEnumDto(this string val)
        {
            return (TaskProgressEnumDto) Enum.Parse(typeof (TaskProgressEnumDto), val);

        }
    }
}