using Event.Bus.AzureServiceBus;
using EventBus.Base;
using EventBus.RabbitMQ;
using EventBus.Base.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Factory
{
	public static class EventBusFactory
	{
		public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
		{

			return config.EventBusType switch
			{
				EventBusType.AzureServiceBus => new EventBusServiceBus(serviceProvider,config),
				_ => new EventBusRabbitMQ(serviceProvider, config),
			};
		}
	}
}
