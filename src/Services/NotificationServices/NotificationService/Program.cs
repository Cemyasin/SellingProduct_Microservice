using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using NotificationService.IntegrationEvents.EventHandlers;
using System;

namespace NotificationService
{
	internal class Program
	{
		static void Main(string[] args)
		{
			ServiceCollection services = new ServiceCollection();

			ConfigureServices(services);



			var sp =  services.BuildServiceProvider();

			IEventBus eventBus = sp.GetService<IEventBus>();

			eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
			eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();


			Console.WriteLine("Application is Running.......");

			Console.ReadLine();
		}

		private static void ConfigureServices(ServiceCollection services)
		{
			services.AddLogging(configure =>
			{
				configure.AddConsole();

			});

			services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();
			services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();

			services.AddSingleton<IEventBus>(sp =>
			{
				EventBusConfig config = new()
				{
					ConnectionRetrycount = 5,
					EventNameSuffix = "IntegrationEvent",
					SubscriberClientappName = "NotificationService",
					EventBusType = EventBusType.RabbitMQ
				};

				return EventBusFactory.Create(config, sp);
			});
		}
	}
}
