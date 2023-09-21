using OrderService.Domain.Exceptions;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.AggregateModels.BuyerAggregate
{
	public class PaymentMethod : BaseEntity
	{
		public int					CardTypeId					{ get; set; }
        public string				Alias						{ get; set; }
		public string				CardNumber					{ get; set; }
		public string				SecurityNumber				{ get; set; }
		public string				CardHolderName				{ get; set; }
		public DateTime				Expiration					{ get; set; }

		public CardType				CardType					{ get; private set; }

		public PaymentMethod()
		{ }

		public PaymentMethod(int cardTypeId, string alias, string cardnumber, string securityNumber, string cardHolderName, DateTime expieation)
		{

			CardNumber		= !string.IsNullOrWhiteSpace( cardnumber ) ? cardnumber : throw new OrderingDomainException(nameof(cardnumber));
			SecurityNumber	= !string.IsNullOrWhiteSpace( securityNumber ) ? securityNumber : throw new OrderingDomainException(nameof (securityNumber));
			CardHolderName	= !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));

			if(expieation < DateTime.UtcNow)
			{
				throw new OrderingDomainException(nameof(expieation));
			}

			Alias = alias;
			Expiration = expieation;
			CardTypeId = cardTypeId;
		}


		public bool IsEqualTo(int cardTypeId, string cardnumber, DateTime expieation)
		{
			return CardTypeId == cardTypeId
				&& CardNumber == cardnumber
				&& Expiration == expieation;
		}
	}
}
