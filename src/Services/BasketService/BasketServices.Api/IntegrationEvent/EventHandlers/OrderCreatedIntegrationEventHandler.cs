using BasketService.Api.Core.Application.Repository;
using BasketService.Api.IntegrationEvent.Events;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BasketService.Api.IntegrationEvent.EventHandlers
{
	public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
	{
		private readonly IBasketRepository repository;
		private readonly ILogger<OrderCreatedIntegrationEvent> logger;

		public OrderCreatedIntegrationEventHandler(IBasketRepository repository, ILogger<OrderCreatedIntegrationEvent> logger)
		{
			this.repository = repository;
			this.logger		= logger;
		}

		public async Task Handle(OrderCreatedIntegrationEvent @event)
		{
			logger.LogInformation("------- Handling integration event: {IntegrationEventId} at BasketService.Api - ({@IntegrationEvent})", @event.Id);

			await repository.DeleteBasketAsync(@event.UserId.ToString());
		}
	}
}
