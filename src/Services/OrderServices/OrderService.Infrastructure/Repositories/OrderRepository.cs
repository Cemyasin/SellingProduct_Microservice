﻿using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
	public class OrderRepository : GenericRepository<Order>, IOrderRepository
	{
		private readonly OrderDbContext context;
		public OrderRepository(OrderDbContext context) : base(context)
		{
			this.context = context;
		}

		public override async Task<Order> GetByIdAsync(Guid id, params Expression<Func<Order, object>>[] includes)
		{
			var entity = await base.GetByIdAsync(id, includes);

			if(entity == null)
			{
				entity = context.Orders.Local.FirstOrDefault(i => i.Id == id);
			}
			return entity;
		}
	}
}
