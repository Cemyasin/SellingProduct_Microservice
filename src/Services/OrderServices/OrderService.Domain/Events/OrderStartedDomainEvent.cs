using MediatR;
using OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Events
{
	public class OrderStartedDomainEvent : INotification
	{

        public string			UserName					{ get; }
																   
        public string			CardNumber					{ get; }
																   
		public string			CardHolderName				{ get; }
																   
		public DateTime			CardExpiration				{ get; }
																   
		public string			CardSecurityNumber			{ get; }
        														   
		public int				CardTypeId					{ get; }
																   
		public Order			Order						{ get; }

		public OrderStartedDomainEvent(Order order,string userName, string cardNumber, 
			string cardHolderName, DateTime cardExpiration, 
			string cardSecurityNumber, int cardTypeId)
		{
			UserName			=		userName;
			CardNumber			=		cardNumber;
			CardHolderName	    =		cardHolderName;
			CardExpiration		=		cardExpiration;
			CardSecurityNumber	=		cardSecurityNumber;
			CardTypeId			=		cardTypeId;
			Order				=		order;
		}
	}
}
