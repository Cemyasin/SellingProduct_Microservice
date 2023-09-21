using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.AggregateModels.BuyerAggregate
{
	public class Buyer : BaseEntity, IAggregateRoot
	{
		public string Name { get; set; }

		private List<PaymentMethod> paymentMethods; 

		public IEnumerable<PaymentMethod> PaymentMethods  => paymentMethods.AsReadOnly();

		protected Buyer()
		{
			paymentMethods = new List<PaymentMethod>();
		}

		public Buyer(string name): this()
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public PaymentMethod VerifyOrAddPaymentMethod(
			int cardTypeId, string alias, string cardNumber,
			string securityNumber,string cardHolderNumber, DateTime expiration, Guid orderId)
		{
			var existingPayment = paymentMethods.SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

			if(existingPayment != null)
			{
				AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));
				
				return existingPayment;
			}

			var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderNumber, expiration);
			
			paymentMethods.Add(payment);

			//raise event
			AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

			return payment;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj) || 
				(obj is Buyer buyer &&
				Id.Equals(buyer.Id) && Name == buyer.Name);
		}
	}
}
