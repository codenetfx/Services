using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Defines operatrions to manage data rules.
    /// </summary>
    public interface IDataRuleManager<in T> where T:IDataRuleContext
    {
        ///// <summary>
        ///// Processes the specified context.
        ///// </summary>
        ///// <param name="context">The context.</param>
        ///// <returns></returns>
        //bool Process(T context);

        /// <summary>
        /// Process2s the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        bool Process(T context);
    }
}
