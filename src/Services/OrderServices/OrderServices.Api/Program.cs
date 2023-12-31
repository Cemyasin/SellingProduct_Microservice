using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Infrastructure.Context;
using OrderServices.Api.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//CreateHostBuilder(args).Build().Run();

			var host = BuildWebHost(GetConfiguration(), args);

			host.MigrateDbContext<OrderDbContext>((context, services) =>
			{
				var logger = services.GetService<ILogger<OrderDbContext>>();

				var dbContextSeeder = new OrderDbContextSeed();
				dbContextSeeder.SeedAsync(context, logger)
				.Wait();
			});
		}

		static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.UseDefaultServiceProvider((context, options) =>
			{
				options.ValidateOnBuild = false;
			})
			.ConfigureAppConfiguration(i => i.AddConfiguration(configuration))
			.UseStartup<Startup>()
			.Build();

		//public static IHostBuilder CreateHostBuilder(string[] args) =>
		//	Host.CreateDefaultBuilder(args)
		//		.ConfigureWebHostDefaults(webBuilder =>
		//		{
		//			webBuilder.UseStartup<Startup>();
		//		});


		static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();

			return builder.Build();
		}

	}
}
