using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvent.Events;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BasketService.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository			repository;
		private readonly IIdentityService			identityService;
		private readonly IEventBus					eventBus;
		private readonly ILogger<BasketController>	logger;

		public BasketController(
			IBasketRepository			repository,
			IIdentityService			identityService,
			IEventBus					eventBus,
			ILogger<BasketController>	logger)
		{
			this.repository			=	repository;
			this.identityService	=	identityService;
			this.eventBus			=	eventBus;
			this.logger				=	logger;
		}


		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Basket Service is Up and Running");
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
		{
			CustomerBasket basket = await repository.GetBasketAsync(id);

			return Ok(basket ?? new CustomerBasket(id));
		}


		[HttpPost]
		[Route("update")]
		[ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
		{
			 return Ok(await repository.UpdateBasketAsync(value));
		}



		[HttpPost]
		[Route("additem")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<CustomerBasket>> AddItemToBasket([FromBody] BasketItem basketItem)
		{
			string userId			= identityService.GetUserName().ToString();

			CustomerBasket basket	= await repository.GetBasketAsync(userId);

			if(basket == null) 
			{
				basket = new CustomerBasket(userId);
			}

			basket.Items.Add(basketItem);

			await repository.UpdateBasketAsync(basket);

			return Ok();
		}



		[HttpPost]
		[Route("checkout")]
		[ProducesResponseType((int)HttpStatusCode.Accepted)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<CustomerBasket>> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
		{
			string userId			= basketCheckout.Buyer;

			CustomerBasket basket	= await repository.GetBasketAsync(userId);

			if (basket == null) return BadRequest();

			string userName			= identityService.GetUserName();

			var eventMessage		= new OrderCreatedIntegrationEvent(userId, userName, basketCheckout.City, basketCheckout.Street, basketCheckout.State,
				basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName, basketCheckout.CardExpiration,
				basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.Buyer, basket);


			try
			{
				eventBus.Publish(eventMessage);
			}catch(Exception ex)
			{
				logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {BasketService.App}", eventMessage.Id);

				throw;
			}

			return Accepted();
		}



		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task DeleteBasketByIdAsync(string id)
		{
			await repository.DeleteBasketAsync(id);
		}


	}
}
