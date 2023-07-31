using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Api.Infrastructure.Repository
{
	public class RedisBasketRepository : IBasketRepository
	{

		private readonly ILogger<RedisBasketRepository> logger;
		private readonly ConnectionMultiplexer redis;
		private readonly IDatabase database;

		public RedisBasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
		{
			this.logger		= loggerFactory.CreateLogger<RedisBasketRepository>();
			this.redis		= redis;
			database		= redis.GetDatabase();
		}

		public async Task<bool> DeleteBasketAsync(string id)
		{
			return await database.KeyDeleteAsync(id);
		}

		public async Task<CustomerBasket> GetBasketAsync(string customerId)
		{
			var data = await database.StringGetAsync(customerId);

			if (data.IsNullOrEmpty) return null;

			return JsonConvert.DeserializeObject<CustomerBasket>(data);
		}

		public IEnumerable<string> GetUsers()
		{
			var server	= GetServer();
			var data	= server.Keys();

			return data?.Select(x => x.ToString());
		}

		public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
		{
			var created = await database.StringSetAsync(basket.BuyerId,JsonConvert.SerializeObject(basket));

			if (!created)
			{
				logger.LogInformation("Problem occur persisting the item.");
				return null;
			}

			logger.LogInformation("Basket item persisted succesfully.");

			return await GetBasketAsync(basket.BuyerId);
		}

		private IServer GetServer()
		{
			var endpoint = redis.GetEndPoints();
			return redis.GetServer(endpoint.First());
		}
	}
}
