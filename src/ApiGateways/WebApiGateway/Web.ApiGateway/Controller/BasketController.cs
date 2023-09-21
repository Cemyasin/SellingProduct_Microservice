using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.ApiGateway.Models.Basket;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Controller
{
    [Route("[controller]")]
    [ApiController]
   // [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;
        private readonly ICatalogService catalogService;

        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            this.basketService = basketService;
            this.catalogService = catalogService;
        }

        [HttpPost]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddBasketItemAsync([FromBody] AddBasketItemRequest request)
        {
            if(request is null || request.Quantity == 0)
            {
                return BadRequest("Invalid Payload!!");
            } 

            var item = await catalogService.GetCatalogItemAsync(request.CatalogItemId);

            var currentBasket = await basketService.GetById(request.BasketId);

            var product = currentBasket.Items.SingleOrDefault(i => i.ProductId == item.Id);

            if(product is not null)
            {
                product.Quantity += request.Quantity;
            }
            else
            {
                currentBasket.Items.Add(new BasketDataItem()
                {
                    UnitPrice = item.Price,
                    PictureUrl = item.PictureUrl,
                    ProductId = item.Id,
                    Quantity = request.Quantity,
                    Id = Guid.NewGuid().ToString(),
                    ProductName = item.Name 
                });
            }

           await basketService.UpdateAsync(currentBasket);
            return Ok();
        }
    }
}
