using UL.Aria.Common.Authorization;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.View;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     defines contract for smtp client management
    /// </summary>
    public interface ISmtpClientManager
    {
        /// <summary>
        ///     Builds and sends the Contact Us email
        /// </summary>
        /// <param name="contact">The contact.</param>
        void ContactUs(ContactUs contact);

	    /// <summary>
	    ///     Sends the by default settings.
	    /// </summary>
	    /// <param name="product"></param>
	    void SendProductStatusChanged(Product product);

        /// <summary>
        ///     Sends the portal access request.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        void SendPortalAccessRequest(string loginId);

        /// <summary>
        ///     Sends the portal access granted.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        void SendPortalAccessGranted(string loginId);

        /// <summary>
        /// Sends the notification to create new company.
        /// </summary>
        /// <param name="model">The model to populate email template.</param>
        void SendNewCompany(NewCompanyEmail model);
               
        /// <summary>
        ///     Sends the project created email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="model">The model</param>
        void SendProjectCreated(string user, ProjectEmail model);
                      
        /// <summary>
        ///     Sends the account created email.
        /// </summary>
        /// <param name="user">The user.</param>
        void SendAccountCreated(ProfileBo user);

        /// <summary>
        /// Sends the task completed.
        /// </summary>
        /// <param name="loginId">The login identifier.</param>
        /// <param name="taskCompletedEmail">The task completed email.</param>
        void SendTaskCompleted(string loginId, TaskCompletedEmail taskCompletedEmail);

        /// <summary>
        ///     Sends the portal task assign.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <param name="taskEmail">The task email.</param>
        void SendTaskAssigned(string loginId, TaskEmail taskEmail);

        /// <summary>
        ///     Sends the portal task reassign.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <param name="taskEmail">The task email.</param>
        void SendTaskReassigned(string loginId, TaskEmail taskEmail);

        /// <summary>
        ///     Sends the portal task delete.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <param name="taskEmail">The task email.</param>
        void SendTaskDelete(string loginId, TaskEmail taskEmail);

        /// <summary>
        /// Sends the project assigned.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="model">The model.</param>
        void SendProjectAssigned(string recipient, ProjectAssignmentEmail model);

        /// <summary>
        /// Sends the project assigned.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="model">The model.</param>
        void SendProjectAssignedNewHandler(string recipient, ProjectAssignmentEmail model);

	    /// <summary>
	    /// This method sends out an email to the recipient (project handler and/or order owner) when the project is completed.
	    /// </summary>
	    /// <param name="user">user</param>
	    /// <param name="model">model</param>
	    void SendProjectCompleted(string user, ProjectCompletedEmail model);

	    /// <summary>
	    /// This method sends out an email to the recipient when the project order owner is reassigned.
	    /// </summary>
	    /// <param name="user">user</param>
	    /// <param name="model">model</param>
	    void SendProjectOrderOwnerReassigned(string user, ProjectOrderOwnerReassignedEmail model);
    }
}