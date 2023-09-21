using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
	public class OrderItemsEntityConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{

			builder.ToTable("orderItems", OrderDbContext.DEFAULT_SCHEMA);

			builder.HasKey(o => o.Id);

			builder.Property(i => i.Id).ValueGeneratedOnAdd();

			builder.Ignore(i => i.DomainEvents);

			builder.Property<int>("OrderId").IsRequired();
		}
	}
}
