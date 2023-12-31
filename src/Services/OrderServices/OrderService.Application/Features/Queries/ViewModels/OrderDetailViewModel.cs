﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Queries.ViewModels
{
	public class OrderDetailViewModel
	{


		public string			OrderNumber			{ get; init; }
		
		public DateTime			Date				{ get; init; }
		
		public string			Status				{ get; init; }
		
		public string			Description			{ get; init; }
		
		public string			City				{ get; init; }

		public string			Street				{ get; init; }

		public string			State				{ get; init; }

		public string			Country				{ get; init; }

		public string			ZipCode				{ get; init; }	

		public decimal			Total				{ get; init; }	

		public List<OrderItem> OrderItems			{ get; set; }

	}

	public class OrderItem
	{
		public string			ProductName			{ get; init; }
		public int				Units 				{ get; init; }
		public double			UnitPrice    		{ get; init; }
		public string			PictureUrl			{ get; init; }
	}
}
