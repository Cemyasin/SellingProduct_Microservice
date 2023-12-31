﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.EntityConfigurations;
using OrderService.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Context
{
	public  class OrderDbContext : DbContext, IUnitOfWork
	{
		public const string DEFAULT_SCHEMA = "ordering";
		private readonly IMediator mediator;

        public OrderDbContext(): base()
        {
            
        }

		public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator) : base(options)
		{
			this.mediator = mediator;
		}

		public DbSet<Order>					Orders						{ get; set; }

		public DbSet<OrderItem>				OrderItems					{ get; set; }

		public DbSet<PaymentMethod>			PaymentMethods				{ get; set; }

		public DbSet<Buyer>					Buyers						{ get; set; }
																		
		public DbSet<CardType>				CardTypes					{ get; set; }	
																		
		public DbSet<OrderStatus>			OrderStatuses				{ get; set; }

		public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
		{
			await mediator.DispatchDomainEventAsync(this);

			await base.SaveChangesAsync(cancellationToken);

			return true;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
			modelBuilder.ApplyConfiguration(new BuyerEntityConfiguration());
			modelBuilder.ApplyConfiguration(new CardTypeEntityConfiguration());
			modelBuilder.ApplyConfiguration(new OrderItemsEntityConfiguration());
			modelBuilder.ApplyConfiguration(new OrderStatusEntityConfiguration());
			modelBuilder.ApplyConfiguration(new PaymentMethodEntityConfiguration());
		}

	}
}
