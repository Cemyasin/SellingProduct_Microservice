using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Application.Services.Dtos;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models.ViewModels;
using WebApp.Extensions;

namespace WebApp.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient httpClient;
        private readonly IIdentityService identityService;
        private readonly ILogger<BasketService> logger;

        public BasketService(HttpClient httpClient, IIdentityService identityService, ILogger<BasketService> logger)
        {
            this.httpClient      = httpClient     ;
            this.identityService = identityService;
            this.logger          = logger         ;
        }

        public async Task AddItemToBasket(int productId)
        {
            var model = new
            {
                CatalogItemId = productId,
                Quantity = 1,
                BasketId = identityService.GetUserName(),
            };
            await httpClient.PostAsync("basket/items", model);
        }

        public Task Checkout(BasketDTO basket)
        { 
            return httpClient.PostAsync("basket/checkout", basket);
        }

        public async Task<Basket> GetBasket()
        {
            Basket response = await httpClient.GetResponseAsync<Basket>("basket/" + identityService.GetUserName());

            return response ?? new Basket() { BuyerId = identityService.GetUserName() };
        }

        public async Task<Basket> UpdateBasket(Basket basket)
        {
            Basket response = await httpClient.PostGetResponseAsync<Basket, Basket>("basket/update", basket);

            return response;
        }
    }
}
