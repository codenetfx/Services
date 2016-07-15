using System.Net.Mail;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// contract for Smtp Client Provider
    /// </summary>
    public interface ISmtpClientProvider
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorMessage"></param>
        bool Send(MailMessage message, out string errorMessage);
    }
}