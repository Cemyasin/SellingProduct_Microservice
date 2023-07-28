﻿using EventBus.Base.Events;

namespace PaymentService.Api.IntegrationEvents.Events
{
	public class OrderStartedIntegrationEvent : IntegrationEvent
	{
        public int OrderId { get; set; }
        public string ErrorMessage { get; set; }
        public OrderStartedIntegrationEvent()
        {
            
        }

		public OrderStartedIntegrationEvent(int orderId, string errormessage)
		{
			OrderId = orderId;
			ErrorMessage = errormessage;
		}
	}
}
