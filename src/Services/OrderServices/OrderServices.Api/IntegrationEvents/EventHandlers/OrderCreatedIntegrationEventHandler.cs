using EventBus.Base.Abstraction;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Features.Commands.CreateOrder;
using OrderServices.Api.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace OrderServices.Api.IntegrationEvents.EventHandlers
{
	public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
	{

		private readonly IMediator mediator;
		private readonly ILogger logger;

		public OrderCreatedIntegrationEventHandler(IMediator mediator, ILogger logger)
		{
			this.mediator = mediator;
			this.logger = logger;
		}

		public async Task Handle(OrderCreatedIntegrationEvent @event)
		{
			logger.LogInformation("Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, typeof(Startup).Namespace, @event);

			var createOrderCommand = new CreateOrderCommand(@event.Basket.Items,@event.UserId, @event.UserName, @event.City,
				@event.Street, @event.State, @event.Country, @event.ZipCode, @event.CardNumber, @event.CardSecurityNumber,
				@event.CardHolderName, @event.Expiration);

			await mediator.Send(createOrderCommand);

		}
	}
}
