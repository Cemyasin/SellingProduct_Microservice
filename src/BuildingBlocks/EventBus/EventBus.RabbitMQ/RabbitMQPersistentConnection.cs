using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
	public class RabbitMQPersistentConnection : IDisposable
	{
		private readonly IConnectionFactory connectionFactory;
		private IConnection connection;
		private object lock_object = new object();
		private int retryCount;
		private bool disposed;

		public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
		{
			this.connectionFactory = connectionFactory;
			this.retryCount = retryCount;
		}

		public bool IsConnected =>connection != null && connection.IsOpen;

		public IModel CreateModel()
		{
			return connection.CreateModel();
		}
		public void Dispose()
		{
			disposed = true;
			connection.Dispose();
		}

		public bool TryConnect()
		{
			lock(lock_object)
			{
				var policy = Policy.Handle<SocketException>()
					.Or<BrokerUnreachableException>()
					.WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
					{

					});

				policy.Execute(() =>
				{
					connection = connectionFactory.CreateConnection();
				});

				if (IsConnected)
				{
					connection.ConnectionShutdown += Connection_ConnectionShutDown;
					connection.CallbackException  += Connection_CallBackException;
					connection.ConnectionBlocked  += Connection_ConnectionBlock;

					//log
					
					return true;
				}
			}
			return false;
		}

		private void Connection_ConnectionBlock(object sender, ConnectionBlockedEventArgs e)
		{
			if (disposed)	return;
			TryConnect();
		}

		private void Connection_CallBackException(object sender, CallbackExceptionEventArgs e)
		{
			if (disposed) return;
			TryConnect();
		}

		private void Connection_ConnectionShutDown(object sender, ShutdownEventArgs e)
		{
			// log 
			if (disposed) return;

			TryConnect();
		}
	}
}
