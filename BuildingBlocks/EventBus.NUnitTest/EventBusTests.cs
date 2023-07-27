using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.NUnitTest.Events.EventHandlers;
using EventBus.NUnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Diagnostics;

namespace EventBus.NUnitTest
{
	public class EventBusTests
	{

		private ServiceCollection services;

		public EventBusTests()
		{
			this.services = new ServiceCollection();
			services.AddLogging(configure => configure.AddConsole());

		}

		[Test]
		public void subscribe_event_on_rabbitmq_test()
		{

			services.AddSingleton<IEventBus>(sp =>
			{

				return EventBusFactory.Create(GetRabbitMQConfig(), sp);
			});

			
			var sp = services.BuildServiceProvider();

			var eventBus = sp.GetRequiredService<IEventBus>();

			eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
			eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
		}


		[Test]
		public void subscribe_event_on_azure_test()
		{

			services.AddSingleton<IEventBus>(sp =>
			{

				return EventBusFactory.Create(GetAzureConfig(), sp);
			});


			var sp = services.BuildServiceProvider();

			var eventBus = sp.GetRequiredService<IEventBus>();

			eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
			eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
		}

		[Test]
		public void send_message_to_rabbitmq()
		{
			services.AddSingleton<IEventBus>(sp =>
			{

				return EventBusFactory.Create(GetRabbitMQConfig(), sp);
			});


			var sp = services.BuildServiceProvider();

			var eventBus = sp.GetRequiredService<IEventBus>();

			eventBus.Publish(new OrderCreatedIntegrationEvent(1));
		}

		[Test]
		public void send_message_to_azure()
		{
			services.AddSingleton<IEventBus>(sp =>
			{

				return EventBusFactory.Create(GetAzureConfig(), sp);
			});


			var sp = services.BuildServiceProvider();

			var eventBus = sp.GetRequiredService<IEventBus>();

			eventBus.Publish(new OrderCreatedIntegrationEvent(1));
		}

		private EventBusConfig GetAzureConfig()
		{
			return new EventBusConfig()
			{
				ConnectionRetrycount = 5,
				SubscriberClientappName = "EventBus.UnitTest",
				DefaultTopicName = "SellingProductTopicName",
				EventBusType = EventBusType.AzureServiceBus,
				EventNameSuffix = "IntegrationEvent",
				EventBusConnectionString = "constring"
			};
		}

		private EventBusConfig GetRabbitMQConfig()
		{
			return new EventBusConfig()
			{
				ConnectionRetrycount = 5,
				SubscriberClientappName = "EventBus.UnitTest",
				DefaultTopicName = "SellingProductTopicName",
				EventBusType = EventBusType.RabbitMQ,
				EventNameSuffix = "IntegrationEvent",
				//Connection = new ConnectionFactory()
				//{
				//	HostName = "localhost",
				//	Port = 5672,
				//	UserName = "guest",
				//	Password = "guest",
				//}
			};
		}

	}
}