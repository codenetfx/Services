using System;
using System.Net.Mail;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     defines operations to fulfill email capabilities
    /// </summary>
    public class SmtpClientProvider : ISmtpClientProvider
    {
        /// <summary>
        ///     Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorMessage"></param>
        public bool Send(MailMessage message, out string errorMessage)
        {
            errorMessage = String.Empty;

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            return true;
        }
    }
}