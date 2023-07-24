using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.Events.EventHandlers;
using EventBus.UnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;

namespace EventBus.UnitTest
{
	[TestClass]
	public class EventBusTests
	{

		private ServiceCollection services;

		public EventBusTests()
		{
			this.services = new ServiceCollection();
			services.AddLogging(configure => configure.AddConsole());

		}

		[TestMethod]
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
		private EventBusConfig GetAzureConfig()
		{
			return  new EventBusConfig()
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
