using System;
using System.Globalization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class TaskStatusEnumDtoExtensions
    /// </summary>
    public static class TaskStatusEnumDtoExtensions
    {
        /// <summary>
        ///     To the share point string.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>System.String.</returns>
        public static string ToSharePointString(this TaskStatusEnumDto val)
        {
            return ((int) val).ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');
        }

        /// <summary>
        ///     To the task status enum dto.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>TaskStatusEnumDto.</returns>
        public static TaskStatusEnumDto FromSharePointStringToTaskStatusEnumDto(this string val)
        {
            return (TaskStatusEnumDto) Convert.ToInt32(val);
        }

        /// <summary>
        ///     To the task status enum dto.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>TaskStatusEnumDto.</returns>
        public static TaskStatusEnumDto ToTaskStatusEnumDto(this string val)
        {
            return (TaskStatusEnumDto) Enum.Parse(typeof (TaskStatusEnumDto), val);
        }
    }
}