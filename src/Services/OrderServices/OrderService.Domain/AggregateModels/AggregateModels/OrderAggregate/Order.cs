using OrderService.Domain.AggregateModels.AggregateModels.BuyerAggregate;
using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate
{
	public class Order :BaseEntity, IAggregateRoot
	{

		private int orderStatusId;
		public DateTime		OrderDate			{ get; private set; }

        public int			Quantity			{ get; private set; }

        public string		Description			{ get; private set; }

		public Guid?		BuyerId				{ get; private set; }
	
		public Buyer		Buyer				{ get; private set; }

		public Address		Address				{ get; private set; }

		public OrderStatus	OrderStatus			{ get; private set; }

		private readonly List<OrderItem> orderItems;

		public IReadOnlyCollection<OrderItem> OrderItems => orderItems;

		public Guid? PaymentMethodId { get; set; }

		protected Order()
		{
			Id = Guid.NewGuid();
			orderItems = new List<OrderItem>();
		}

		public Order(string userName,Address address,
			int cardTypeId,string cardnumber,
			string cardSecurrityNumber,string cardHolderName,
			DateTime cardExpiration,Guid? paymentMethodId,
			Guid? buyerId = null) : this()
		{
			BuyerId = buyerId;
			orderStatusId = OrderStatus.Submitted.Id;
			OrderDate = DateTime.UtcNow;
			Address = address;
			PaymentMethodId = paymentMethodId;

			AddOrderStartedDomainEvent(userName, cardTypeId,cardnumber,cardSecurrityNumber,cardHolderName,cardExpiration);
		}

		private void AddOrderStartedDomainEvent(string userName, int cardTypeId, string cardnumber, string cardSecurrityNumber, string cardHolderName, DateTime cardExpiration)
		{
			var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userName, cardnumber, cardHolderName, cardExpiration, cardSecurrityNumber, cardTypeId);
			
			this.AddDomainEvent(orderStartedDomainEvent);
		}


		public void AddOrderItem(int productId,string productName, decimal unitPrice, string pictureUrl, int units = 1)
		{
			//orderItem validations

			var orderItem = new OrderItem(productId,productName,pictureUrl,unitPrice,units);

			orderItems.Add(orderItem);
		}

		public void SetBuyerId(Guid buyerId)
		{
			BuyerId = buyerId;
		}

		public void SetPaymentMethodId(Guid paymentMethodId)
		{
			PaymentMethodId = paymentMethodId;
		}
		
	}
}
