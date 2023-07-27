using CatalogService.Api.Core.Application.ViewModels;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace CatalogService.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		private readonly CatalogContext catalogContext;
		private readonly CatalogSettings catalogSettings;

		public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
		{
			this.catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
			this.catalogSettings = settings.Value;

			catalogContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}



		//GET api/v1/[controller]/items[?pageSize3&pageIndex=10]
		[HttpGet]
		[Route("items")]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
		{
			if (!string.IsNullOrEmpty(ids))
			{
				var items = await GetItemsByIdsAsync(ids);

				if (!items.Any())
				{
					return BadRequest("ids value invalid. Must be comma-separated list of numbers");
				}

				return Ok(items);
			}

			var totalItems = await catalogContext.CatalogItems.LongCountAsync();

			var itemsOnPage = await catalogContext.CatalogItems
				.OrderBy(c => c.Name)
				.Skip(pageSize * pageIndex)
				.Take(pageSize)
				.ToListAsync();

			itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

			var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

			return Ok(model);
		}


		private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
		{
			var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

			if (!numIds.All(nid => nid.Ok))
			{
				return new List<CatalogItem>();
			}

			var idsToSelect = numIds.Select(id => id.Value);

			var items = await catalogContext.CatalogItems.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

			items = ChangeUriPlaceholder(items);

			return items;
		}


		[HttpGet]
		[Route("items/{id:int}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<CatalogItem>> ItemByIdAsync(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			var item = await catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == id);

			var baseUri = catalogSettings.PicBaseUrl;

			if (item != null)
			{
				item.PictureUri = baseUri + item.PictureFileName;

				return item;
			}
			return NotFound();
		}


		//Get api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
		[HttpGet]
		[Route("items/withname/{name:minlength(1)}")]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsWithNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
		{
			var totalItems = await catalogContext.CatalogItems
			   .Where(ci => ci.Name.StartsWith(name))
			   .LongCountAsync();


			var itemsOnPage = await catalogContext.CatalogItems
				.Where(c => c.Name.StartsWith(name))
				.Skip(pageSize * pageIndex)
				.Take(pageSize)
				.ToListAsync();

			itemsOnPage = ChangeUriPlaceholder(itemsOnPage);
			
			return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
		}

		//Get api/v1/[controller]/items/type/1/brand[?pageSize=3&pageIndex=10]
		[HttpGet]
		[Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandIdAsync(int catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 10)
		{
			var root = (IQueryable<CatalogItem>)catalogContext.CatalogItems;

			root = root.Where(ci => ci.CatalogTypeId == catalogTypeId);

			if (catalogBrandId.HasValue)
			{
				root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);
			}

			var totelItems = await root.LongCountAsync();

			var itemsOnPage = await root
				.Skip(pageSize * pageIndex)
				.Take(pageSize)
				.ToListAsync();

			itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

			return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totelItems, itemsOnPage);
		}


		//Get api/v1/[controller]/items/type/all/brand[?pageSize=3&pageIndex=10]
		[HttpGet]
		[Route("items/type/all/brand/{catalogBrandId:int?}")]
		[ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByBrandIdAsync(int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 10)
		{
			var root = (IQueryable<CatalogItem>)catalogContext.CatalogItems;

			if (catalogBrandId.HasValue)
			{
				root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);
			}

			var totelItems = await root.LongCountAsync();

			var itemsOnPage = await root
				.Skip(pageSize * pageIndex)
				.Take(pageSize)
				.ToListAsync();

			itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

			return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totelItems, itemsOnPage);
		}


		//GET api/v1/[controller]/CatalogTypes
		[HttpGet]
		[Route("catalogtypes")]
		[ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<List<CatalogType>>> CatalogTypesAsync()
		{
			return await catalogContext.CatalogTypes.ToListAsync();
		}

		//GET api/v1/[controller]/CatalogBrands
		[HttpGet]
		[Route("catalogbrands")]
		[ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<List<CatalogBrand>>> CatalogBrandsAsync()
		{
			return await catalogContext.CatalogBrands.ToListAsync();
		}


		//PUT api/v1/[controller]/items
		[Route("items")]
		[HttpPut]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult> UpdateProductAsync([FromBody] CatalogItem productToUpdate)
		{
			var catalogItem = await catalogContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

			if (catalogItem == null)
			{
				return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
			}

			var oldPrice = catalogItem.Price;
			var raiseProductPriceChangeEvent = oldPrice != productToUpdate.Price;

			catalogItem = productToUpdate;

			catalogContext.CatalogItems.Update(catalogItem);

			await catalogContext.SaveChangesAsync();

			return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
		}



		//POST api/v1/[controller]/items
		[Route("items")]
		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult> CreateProductAsync([FromBody] CatalogItem product)
		{
			var item = new CatalogItem
			{
				CatalogBrandId = product.CatalogBrandId,
				CatalogTypeId = product.CatalogTypeId,
				Description = product.Description,
				Name = product.Name,
				PictureFileName = product.PictureFileName,
				Price = product.Price,
			};

			catalogContext.CatalogItems.Add(item);

			await catalogContext.SaveChangesAsync();

			return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
		}

		//DELETE api/v1/[controller]/id
		[Route("{id}")]
		[HttpDelete]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<ActionResult> DeleteProductAsync(int id)
		{
			var product = catalogContext.CatalogItems.FirstOrDefault(x => x.Id == id);
			
			if (product == null)
			{
				return NotFound();
			}

			catalogContext.CatalogItems.Remove(product);

			await catalogContext.SaveChangesAsync();	

			return NoContent();
		}

		private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
		{
			var baseUri = catalogSettings.PicBaseUrl;

			foreach (var item in items)
			{
				if(item != null)
				{
					item.PictureUri = baseUri + item.PictureFileName;
				}
			}

			return items;
		}


	}
}
