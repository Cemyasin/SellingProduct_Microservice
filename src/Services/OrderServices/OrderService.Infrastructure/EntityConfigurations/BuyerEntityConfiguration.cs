﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.AggregateModels.BuyerAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
	public class BuyerEntityConfiguration : IEntityTypeConfiguration<Buyer>
	{
		public void Configure(EntityTypeBuilder<Buyer> builder)
		{

			builder.ToTable("buyers", OrderDbContext.DEFAULT_SCHEMA);

			builder.HasKey(b => b.Id);

			builder.Ignore(i => i.DomainEvents);
			builder.Property(i => i.Id).ValueGeneratedOnAdd();

			builder.Property(b => b.Name).HasColumnName("name").HasColumnType("varchar").HasMaxLength(100);


			builder.HasMany(b => b.PaymentMethods)
				.WithOne()
				.HasForeignKey(i => i.Id)
				.OnDelete(DeleteBehavior.Cascade);
			

			var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

			navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		


		}
	}
}
