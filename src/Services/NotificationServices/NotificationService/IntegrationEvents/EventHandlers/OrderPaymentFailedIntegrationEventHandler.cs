using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.IntegrationEvents.EventHandlers
{
	internal class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
	{

		private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> logger;

		public OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
		{
			this.logger = logger;
		}

		public Task Handle(OrderPaymentFailedIntegrationEvent @event)
		{
			//Send Fail Notification (Sms, EMail, Push)

			logger.LogInformation($"Order Payment failed with OrderId: {@event.OrderId}, Message: {@event.ErrorMessage}");

			return Task.CompletedTask;
		}
	}
}
