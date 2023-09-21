using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Context
{
	public class OrderDbContextDeisgnFactory : IDesignTimeDbContextFactory<OrderDbContext>
	{
        public OrderDbContextDeisgnFactory()
        {
            
        }
        public OrderDbContext CreateDbContext(string[] args)
		{
			var connStr = "Data Source=ASUSX550V\\SQLEXPRESS;Initial Catalog=order;Persist Security Info =True;Trusted_Connection=True;TrustServerCertificate=True;";
			//var connStr = "Data Source=c_sqlserver;Initial Catalog=order;Persist Security Info=True;User ID=sa;Password=Salih123!";
			

			var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>()
				.UseSqlServer(connStr);

			return new OrderDbContext(optionsBuilder.Options, new NoMediator());
		}


		class NoMediator : IMediator
		{
			public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
			{
				return (IAsyncEnumerable<TResponse>)Task.CompletedTask;
			}

			public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
			{
				return (IAsyncEnumerable<object>)Task.CompletedTask;
			}

			public Task Publish(object notification, CancellationToken cancellationToken = default)
			{
				return Task.CompletedTask;
			}

			public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
			{
				return Task.CompletedTask;
			}

			public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
			{
				return Task.FromResult<TResponse>(default);
			}

			public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
			{
				return Task.CompletedTask;
			}

			public Task<object> Send(object request, CancellationToken cancellationToken = default)
			{
				return Task.FromResult<object>(default);
			}
		}
	}
}
