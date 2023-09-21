using EventBus.Base.Events;
using OrderService.Domain.Models;
using System;

namespace OrderServices.Api.IntegrationEvents.Events
{
	public class OrderCreatedIntegrationEvent : IntegrationEvent
	{

		public string					UserId						{ get; }
																	
		public string					UserName					{ get; }
																	
        public int						OrderNumber					{ get; set; }
										
		public string					City						{ get; set; }
										
		public string					Street						{ get; set; }
										
		public string					State						{ get; set; }
										
		public string					Country						{ get; set; }
										
		public string					ZipCode						{ get; set; }
										
		public int						CardTypeId					{ get; set; }
										
		public string					CardNumber					{ get; set; }
										
		public string					CardSecurityNumber			{ get; set; }
										
		public string					CardHolderName				{ get; set; }

		public DateTime					Expiration					{ get; set; }

        public string					Buyer						{ get; set; }

		public Guid						RequestId					{ get; set; }

		public CustomerBasket			Basket						{ get; set; }

		public OrderCreatedIntegrationEvent(string userId, string userName, 
			string city, string street, 
			string state, string country, 
			string zipCode, int cardTypeId, 
			string cardNumber, string cardSecurityNumber, 
			string cardHolderName, DateTime expiration, 
			string buyer, Guid requestId, CustomerBasket basket)
		{
			UserId = userId;
			UserName = userName;
			City = city;
			Street = street;
			State = state;
			Country = country;
			ZipCode = zipCode;
			CardTypeId = cardTypeId;
			CardNumber = cardNumber;
			CardSecurityNumber = cardSecurityNumber;
			CardHolderName = cardHolderName;
			Expiration = expiration;
			Buyer = buyer;
			RequestId = requestId;
			Basket = basket;
		}
	}
}
