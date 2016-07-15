using System;
using System.Diagnostics;
using System.ServiceModel;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
	/// <summary>
	///     Implements operations for contacts for an <see cref="IncomingOrderDto">incoming order</see>.
	/// </summary>
	public class ContactProcessor : IContactProcessor
	{
		private readonly ICustomerPartyService _customerPartyService;
		private readonly IIncomingOrderProvider _incomingOrderProvider;
		private readonly ILogManager _logManager;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly IProjectProvider _projectProvider;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		///     Initializes a new instance of the <see cref="ContactProcessor" /> class.
		/// </summary>
		/// <param name="customerPartyService">The customer party service.</param>
		/// <param name="incomingOrderProvider">The incoming order provider.</param>
		/// <param name="projectProvider">The project provider.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="logManager">The log manager.</param>
		public ContactProcessor(
			ICustomerPartyService customerPartyService,
			IIncomingOrderProvider incomingOrderProvider,
			IProjectProvider projectProvider,
			ITransactionFactory transactionFactory,
			IMapperRegistry mapperRegistry,
			ILogManager logManager)
		{
			_customerPartyService = customerPartyService;
			_incomingOrderProvider = incomingOrderProvider;
			_projectProvider = projectProvider;
			_transactionFactory = transactionFactory;
			_mapperRegistry = mapperRegistry;
			_logManager = logManager;
		}

		/// <summary>
		/// Processes the contacts.
		/// </summary>
		/// <param name="contactOrder">The contact order.</param>
		public void Process(ContactOrderDto contactOrder)
		{
			IncomingOrderPartiesDto parties = null;

			if (!Execute(contactOrder,
				() => parties = _customerPartyService.FetchParties(contactOrder.OrderNumber)))
			{
				return;
			}

			if (null == parties)
			{
				var logMessage = new LogMessage(MessagesIds.ContactProcessorPartiesNotFound, LogPriority.Low,
					TraceEventType.Information, "Parties not found for Order", LogCategory.InboundOrderMessageService);
				logMessage.Data.Add("MessageId", contactOrder.MessageId);
				logMessage.Data.Add("Receiver", contactOrder.Receiver);
				logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
				_logManager.Log(logMessage);
				return;
			}

			if (!ExecuteFill(contactOrder, parties.Customer, y => parties.Customer = y))
			{
				var logMessage = new LogMessage(MessagesIds.ContactProcessorUnableToFillCustomer, LogPriority.Low,
					TraceEventType.Information, "Unable to fill Customer for Order", LogCategory.InboundOrderMessageService);
				logMessage.Data.Add("MessageId", contactOrder.MessageId);
				logMessage.Data.Add("Receiver", contactOrder.Receiver);
				logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
				if (null != parties.Customer)
				{
					logMessage.Data.Add("AccountNumber", parties.Customer.AccountNumber);
				}
				_logManager.Log(logMessage);
			}

			if (!ExecuteFill(contactOrder, parties.Agent, y => parties.Agent = y))
			{
				var logMessage = new LogMessage(MessagesIds.ContactProcessorUnableToFillAgent, LogPriority.Low,
					TraceEventType.Information, "Unable to fill Agent for Order", LogCategory.InboundOrderMessageService);
				logMessage.Data.Add("MessageId", contactOrder.MessageId);
				logMessage.Data.Add("Receiver", contactOrder.Receiver);
				logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
				if (null != parties.Agent)
				{
					logMessage.Data.Add("AccountNumber", parties.Agent.AccountNumber);
				}
				_logManager.Log(logMessage);
			}

			var agentContact = parties.Agent != null && parties.Agent.Contact != null ? parties.Agent.Contact : null;
			var customerContact = parties.Customer != null && parties.Customer.Contact != null
				? parties.Customer.Contact
				: null;

			using (var transaction = _transactionFactory.Create())
			{
				try
				{
					var incomingOrderExisting = _incomingOrderProvider.FindByOrderNumber(contactOrder.OrderNumber);
					UpdateIncomingOrderContact(incomingOrderExisting, agentContact, customerContact);
				}
				catch (DatabaseItemNotFoundException ex)
				{
					_logManager.Log(ToLogMessage(ex, MessagesIds.ContactProcessorIncomingOrderDatabaseItemNotFoundException,
						contactOrder));
				}
                UpdateProjectContacts(contactOrder, agentContact, customerContact);
                
				transaction.Complete();
			}
		}

		internal bool ExecuteFill(ContactOrderDto contactOrder, IncomingOrderPartyDto incomingOrderPartyDto,
			Action<IncomingOrderPartyDto> assignmentFunc)
		{
			if (null != incomingOrderPartyDto && !(String.IsNullOrWhiteSpace(incomingOrderPartyDto.AccountNumber)))
			{
				var result = Execute(contactOrder,
					() => incomingOrderPartyDto = _customerPartyService.FillParty(incomingOrderPartyDto, contactOrder.OrderNumber))
				             &&
				             Execute(contactOrder,
					             () =>
						             incomingOrderPartyDto =
							             _customerPartyService.FillContact(incomingOrderPartyDto, contactOrder.OrderNumber));
				assignmentFunc(incomingOrderPartyDto);
				return result;
			}
			return false;
		}

		/// <summary>
		/// Executes the specified incoming order dto.
		/// </summary>
		/// <param name="contactOrder">The contact order.</param>
		/// <param name="action">The action.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		internal bool Execute(ContactOrderDto contactOrder, Action action)
		{
			try
			{
				action();
			}
			catch (DatabaseItemNotFoundException ex)
			{
				_logManager.Log(ToLogMessage(ex, MessagesIds.ContactProcessorPartiesDatabaseItemNotFoundException, contactOrder));
				return false;
			}
			catch (EndpointNotFoundException ex)
			{
				_logManager.Log(ToLogMessage(ex, MessagesIds.ContactProcessorPartiesEndpointNotFoundException, contactOrder));
				return false;
			}
			catch (Exception ex)
			{
				_logManager.Log(ToLogMessage(ex, MessagesIds.ContactProcessorPartiesGeneralException, contactOrder));
				throw;
			}
			return true;
		}

		internal void UpdateIncomingOrderContact(IncomingOrder incomingOrder, IncomingOrderContactDto agentContact,
			IncomingOrderContactDto customerContact)
		{
			if (null != agentContact)
			{
				EnsureId(agentContact, incomingOrder.IncomingOrderContact);
				// multiple maps to avoid immutable id errors
				var mappedAgentContact = _mapperRegistry.Map<IncomingOrderContact>(agentContact);
				mappedAgentContact.ContactRoleId = ContactRoleEnum.Customer;
				_incomingOrderProvider.UpdateContact(mappedAgentContact);
			}
			if (null != customerContact)
			{
				bool isNew;
				incomingOrder.ShipToContact = UpdateShipToContact(incomingOrder.ShipToContact, customerContact, out isNew);
				incomingOrder.ShipToContact.ContactRoleId = ContactRoleEnum.ShipTo;
				if (isNew)
				{
				    incomingOrder.ShipToContact.CreatedDateTime = DateTime.UtcNow;
					_incomingOrderProvider.CreateContact(incomingOrder.ShipToContact);
					_incomingOrderProvider.Update(incomingOrder.Id.GetValueOrDefault(), incomingOrder);
				}
				else
				{
					_incomingOrderProvider.UpdateContact(incomingOrder.ShipToContact);
				}
			}
		}

		private static LogMessage ToLogMessage(Exception ex, int messageId, ContactOrderDto contactOrder)
		{
			var logMessage = ex.ToLogMessage(messageId, LogCategory.InboundOrderMessageService, LogPriority.Low,
				TraceEventType.Verbose);
			logMessage.Data.Add("MessageId", contactOrder.MessageId);
			logMessage.Data.Add("Receiver", contactOrder.Receiver);
			logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
			return logMessage;
		}

		internal static void EnsureId(IncomingOrderContactDto contact, IncomingOrderContact incomingOrderContact)
		{
			if (null != incomingOrderContact &&
			    incomingOrderContact.Id.HasValue)
			{
				contact.Id = incomingOrderContact.Id;
			}
			else
			{
				contact.Id = Guid.NewGuid();
			}
		}

		internal IncomingOrderContact UpdateShipToContact(IncomingOrderContact shipToContact,
			IncomingOrderContactDto customerContact, out bool isNew)
		{
			isNew = false;

			if (null == shipToContact || !shipToContact.Id.HasValue ||
			    shipToContact.Id == default(Guid))
			{
				isNew = true;
				customerContact.Id = Guid.NewGuid();
				var mappedCustomerContact = _mapperRegistry.Map<IncomingOrderContact>(customerContact);
				return mappedCustomerContact;
			}

			shipToContact.FullName = customerContact.FullName;
			shipToContact.PartySiteNumber = customerContact.PartySiteNumber;
            //removed because it does not come through in service call shipToContact.SubscriberNumber = customerContact.SubscriberNumber;
			shipToContact.Phone = customerContact.Phone;
			shipToContact.Email = customerContact.Email;
		    shipToContact.UpdatedDateTime = DateTime.UtcNow;
			return shipToContact;
		}

		internal void UpdateProjectContacts(ContactOrderDto contactOrder, IncomingOrderContactDto agentContact, IncomingOrderContactDto customerContact)
		{
			// Get project(s) if they exist
			//
			try
			{
				var projects = _projectProvider.FetchByOrderNumber(contactOrder.OrderNumber);
				if (null != projects)
				{
					foreach (var project in projects)
					{
					    if (project.IsClosed)
					    {
					        continue;
					    }

						if (null != agentContact)
						{
							EnsureId(agentContact, project.IncomingOrderContact);
							// multiple maps to avoid immutable id errors
							var mappedAgentContact = _mapperRegistry.Map<IncomingOrderContact>(agentContact);
                            mappedAgentContact.IncomingOrderId = project.Id.Value;
							mappedAgentContact.ContactRoleId = ContactRoleEnum.Customer;
                            mappedAgentContact.UpdatedDateTime = DateTime.UtcNow;
							_projectProvider.UpdateContact(project.Id.GetValueOrDefault(), mappedAgentContact);
						}
						if (null != customerContact)
						{
							bool isNew;
                            var updatedCustomerContact = UpdateShipToContact(project.ShipToContact, customerContact, out isNew);
						    updatedCustomerContact.IncomingOrderId = project.Id.Value;
							updatedCustomerContact.ContactRoleId = ContactRoleEnum.ShipTo;

							if (isNew)
							{
							    updatedCustomerContact.CreatedDateTime = DateTime.UtcNow;
								_projectProvider.CreateContact(project.Id.GetValueOrDefault(), updatedCustomerContact);
							}
							else
							{
								_projectProvider.UpdateContact(project.Id.GetValueOrDefault(), updatedCustomerContact);
							}
						}
					}
				}
			}
			catch (DatabaseItemNotFoundException ex)
			{
				_logManager.Log(ToLogMessage(ex, MessagesIds.ContactProcessorProjectsDatabaseItemNotFoundException, contactOrder));
			}
		}
	}
}