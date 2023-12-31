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
using OrderService.Application;
using OrderService.Infrastructure;
using OrderServices.Api.Extensions.Registration.EventHandlerRegistration;
using OrderServices.Api.Extensions.Registration.ServiceDiscovery;
using OrderServices.Api.IntegrationEvents.EventHandlers;
using OrderServices.Api.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Api
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

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderServices.Api", Version = "v1" });
			});

			ConfigureService(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderServices.Api v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			ConfigureEventBusForSubscription(app);
		}

		private void ConfigureService(IServiceCollection services)
		{
			services
				.AddLogging(configure => configure.AddConsole())
				.AddApplicationRegistration(typeof(Startup))
				.AddPersistanceRegistration(Configuration)
				.ConfigureEventHandler()
				.AddServiceDiscoveryRegistration(Configuration);

			services.AddSingleton(sp =>
			{
				EventBusConfig config = new()
				{
					ConnectionRetrycount = 5,
					EventNameSuffix = "IntegrationEvent",
					SubscriberClientappName = "OrderService",
					EventBusType = EventBusType.RabbitMQ
				};

				return EventBusFactory.Create(config, sp);

			});
		}

		private void ConfigureEventBusForSubscription(IApplicationBuilder app)
		{
			var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

			eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
		}

	}
}
