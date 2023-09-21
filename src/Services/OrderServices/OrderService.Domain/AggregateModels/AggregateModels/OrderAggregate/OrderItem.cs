﻿using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate
{
	public class OrderItem : BaseEntity, IValidatableObject
	{

		public int			ProductId						{ get; set; }
		public string		ProductName						{ get; set; }
        public string		PictureUrl						{ get; set; }
        public decimal		UnitPrice						{ get; set; }
        public int			Units							{ get; set; }


        protected OrderItem()
        {
            
        }

		public OrderItem(int productId, string productName, string pictureUrl, decimal unitPrice, int units)
		{
			ProductId		=	productId;
			ProductName		=	productName;
			PictureUrl		=	pictureUrl;
			UnitPrice		=	unitPrice;
			Units			=	units;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();

			if(Units <= 0)
				results.Add(new ValidationResult("Invalid nımber of units", new[] { "Units" }));
			
			return results;
				
			
		}

	}
}
