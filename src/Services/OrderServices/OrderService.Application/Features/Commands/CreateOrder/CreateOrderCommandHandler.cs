using EventBus.Base.Abstraction;
using MediatR;
using OrderService.Application.IntegrationEvents;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Commands.CreateOrder
{
	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
	{


		private readonly IOrderRepository orderRepository;
		private readonly IEventBus eventBus;

		public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
		{
			this.orderRepository = orderRepository;
			this.eventBus = eventBus;
		}


		public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
			var address = new Address(request.City, request.Street, request.State, request.Country, request.ZipCode);

			Order dbOrder = new(request.UserName, address,request.CardTypeId,request.CardNumber,request.CardSecurityNumber,request.CardHolderName,request.CardExpiration,null);

			request.OrderItems.ToList().ForEach(i => dbOrder.AddOrderItem(i.ProductId, i.ProductName, i.UnitPrice,i.PictureUrl, i.Units));

			await orderRepository.AddAsync(dbOrder);
			await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

			var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserName);

			eventBus.Publish(orderStartedIntegrationEvent);

			return true;
		}
	}
}
