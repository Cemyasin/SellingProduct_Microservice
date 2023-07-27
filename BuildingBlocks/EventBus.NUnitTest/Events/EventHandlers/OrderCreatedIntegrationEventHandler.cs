using EventBus.Base.Abstraction;
using EventBus.NUnitTest.Events.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.NUnitTest.Events.EventHandlers
{
	public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
	{
		public Task Handle(OrderCreatedIntegrationEvent @event)
		{
			Debug.WriteLine("Test13579 : "+ @event.Id);
			return Task.CompletedTask;
		}
	}
}
