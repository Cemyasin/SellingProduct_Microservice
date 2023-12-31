using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.IntegrationEvent.EventHandlers;
using BasketService.Api.IntegrationEvent.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketServices.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			ConfigureServicesExt(services);

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketServices.Api", Version = "v1" });
			});
			 

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketServices.Api v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.RegisterWithConsul(lifetime);
			ConfigureSubscription(app.ApplicationServices);
		}


		private void ConfigureServicesExt(IServiceCollection services)
		{
			services.ConfigureAuth(Configuration);

			services.AddSingleton(sp => sp.ConfigureRedis(Configuration));

			services.ConfigureConsul(Configuration);

			services.AddHttpContextAccessor();
			services.AddScoped<IBasketRepository, RedisBasketRepository>();
			services.AddTransient<IIdentityService, IdentityService>();

			services.AddSingleton<IEventBus>(sp =>
			{


				EventBusConfig config = new()
				{
					ConnectionRetrycount = 5,
					EventNameSuffix = "IntegrationEvent",
					SubscriberClientappName = "BasketService",
					EventBusType = EventBusType.RabbitMQ
				};
				return EventBusFactory.Create(config, sp);
			});

			services.AddTransient<OrderCreatedIntegrationEventHandler>();
		}


		private void ConfigureSubscription(IServiceProvider serviceProvider)
		{
			var eventBus = serviceProvider.GetRequiredService<IEventBus>();

			eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
		}
	}
}
