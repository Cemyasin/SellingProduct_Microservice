﻿using Microsoft.Extensions.DependencyInjection;
using OrderServices.Api.IntegrationEvents.EventHandlers;

namespace OrderServices.Api.Extensions.Registration.EventHandlerRegistration
{
	public static class EventHandlerRegistration
	{
		public static IServiceCollection ConfigureEventHandler(this IServiceCollection services)
		{
			services.AddTransient<OrderCreatedIntegrationEventHandler>();

			return services;
		}
	}
}
