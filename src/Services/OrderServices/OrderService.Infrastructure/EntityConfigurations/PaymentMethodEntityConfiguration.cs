﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Domain.AggregateModels.AggregateModels.BuyerAggregate;

namespace OrderService.Infrastructure.EntityConfigurations
{
	public class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethod>
	{
		public void Configure(EntityTypeBuilder<PaymentMethod> builder)
		{

			builder.ToTable("paymentmethods", OrderDbContext.DEFAULT_SCHEMA);

			builder.Ignore(i => i.DomainEvents);

			builder.HasKey(o => o.Id);
			builder.Property(i => i.Id).HasColumnName("id").ValueGeneratedOnAdd();

			builder.Property<int>("BuyerId")
				.IsRequired();

			builder
				.Property(i => i.CardHolderName)
				.UsePropertyAccessMode(PropertyAccessMode.Field)
				.HasColumnName("CardHolderName")
				.HasMaxLength(200)
				.IsRequired();


			builder
				.Property(i => i.Alias)
				.UsePropertyAccessMode(PropertyAccessMode.Field)
				.HasColumnName("Alias")
				.HasMaxLength(200)
				.IsRequired();

			builder
				.Property(i => i.CardNumber)
				.UsePropertyAccessMode(PropertyAccessMode.Field)
				.HasColumnName("CardNumber")
				.HasMaxLength(25)
				.IsRequired();

			builder
				.Property(i => i.Expiration)
				.UsePropertyAccessMode(PropertyAccessMode.Field)
				.HasColumnName("Expiration")
				.HasMaxLength(25)
				.IsRequired();

			builder
				.Property(i => i.CardTypeId)
				.UsePropertyAccessMode(PropertyAccessMode.Field)
				.HasColumnName("CardTypeId")
				.IsRequired();

			builder.HasOne(p => p.CardType)
				.WithMany()
				.HasForeignKey(i => i.CardTypeId);

		}
	}
}
