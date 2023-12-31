﻿using CatalogService.Api.Core.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Api.Infrastructure.Context
{
	public class CatalogContextSeed
	{
		public async Task SeedAsync(CatalogContext context, IWebHostEnvironment env, ILogger<CatalogContextSeed> logger)
		{
			var policy = Policy.Handle<SqlException>()
				.WaitAndRetryAsync(
				retryCount: 3,
				sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
				onRetry: (exception, timeSpan, retry, ctx) =>
				{
					logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attemp {retry} of");
				});

			var setupDirPath = Path.Combine(env.ContentRootPath, "Infrastructure", "Setup", "SeedFiles");
			var picturePath = "Pics";

			await policy.ExecuteAsync(() => ProcessSeeding(context, setupDirPath, picturePath, logger));
		 }

		private async Task ProcessSeeding(CatalogContext context, string setupDirPath, string picturePath, ILogger logger)  
		{
			if(!context.CatalogBrands.Any())
			{
				await context.CatalogBrands.AddRangeAsync(GetCatalogBrandsFromFile(setupDirPath));
				await context.SaveChangesAsync();
			}

			if(!context.CatalogTypes.Any()) 
			{ 
				await context.CatalogTypes.AddRangeAsync(GetCatalogTypesFromFile(setupDirPath));
				await context.SaveChangesAsync();
			}

			if (!context.CatalogItems.Any())
			{
				await context.CatalogItems.AddRangeAsync(GetCatalogItemsFromFile(setupDirPath,context));
				await context.SaveChangesAsync();

				GetCatalogItemPictures(setupDirPath, picturePath);
			}
        }

		private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentpath) 
		{
			IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
			{
				return new List<CatalogType>()
				{
					new CatalogType { Type = "Mug"},
					new CatalogType { Type = "T-Shirt" },
					new CatalogType { Type = "Sheet" },
					new CatalogType { Type = "USB Memory Stick" }
				};
			}

			string fileName = Path.Combine(contentpath, "CatalogTypes.txt");

			if(!File.Exists(fileName))
			{
				return GetPreconfiguredCatalogTypes();
			}

			var fileContent = File.ReadAllLines(fileName);

			var list = fileContent.Select(i => new CatalogType
			{
				Type = i.Trim('"'),
			}).Where(i => i != null);

			return list ?? GetPreconfiguredCatalogTypes();
		}

		private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentpath) 
		{
			IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
			{
				return new List<CatalogBrand>()
				{
					new CatalogBrand { Brand = "Azure"},
					new CatalogBrand { Brand = ".NET"},
					new CatalogBrand { Brand = "Visual Studio"},
					new CatalogBrand { Brand = "SQL Server"},
					new CatalogBrand { Brand = "Other"},
				};
			}

			string fileName = Path.Combine(contentpath, "BrandsTextFile.txt");

			if (!File.Exists(fileName))
			{
				return GetPreconfiguredCatalogBrands();
			}

			var fileContent = File.ReadAllLines(fileName);

			var list = fileContent.Select(i => new CatalogBrand
			{
				Brand = i.Trim('"'),
			}).Where(i => i != null);

			return list ?? GetPreconfiguredCatalogBrands();
		}

		private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentpath, CatalogContext context)  {
			
			IEnumerable<CatalogItem> GetPreconfiguredItems()
			{
				return new List<CatalogItem>()
				{

				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie",  Name =  ".NET Bot Black Hoodie",		Price = 19, OnOrder = false },
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 89 , Description = ".NET Black & White Mug", Name = ".NET Black & White Mug",		Price = 85, OnOrder = true },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 56 , Description = "Prism White T-Shirt", Name = "Prism White T-Shirt",			Price = 8, OnOrder = false },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 120, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt",	Price = 12, OnOrder = false },
				new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 5, AvailableStock = 55 , Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet",					Price = 19, OnOrder = false },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 17 , Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie",					Price = 12, OnOrder = false },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 8  , Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt",				Price = 12, OnOrder = false },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 34 , Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie",				Price = 85, OnOrder = false },
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 76 , Description = "Cup<T> White Mug", Name = "Cup<T> White Mug",					Price = 12, OnOrder = false },new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie", Name = "NET Bot Black Hoodie",Price = 19, OnOrder = false },
				new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 11 , Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet",		Price = 12, OnOrder = false },
				new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 3  , Description = "Cup<T> Sheet", Name = "Cup<T> Sheet",							Price = 85, OnOrder = false },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 0  , Description = "Prism White TShirt", Name = "Prism White TShirt",				Price = 12, OnOrder = false },
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 20 , Description = "De los Palotes", Name = "pepito",								Price = 12, OnOrder = false },

				};
			};

			string fileName = Path.Combine(contentpath, "CatalogItems.txt");

			if(!File.Exists(fileName))
			{
				return GetPreconfiguredItems();
			}

			var catalogTypeIdLookup = context.CatalogTypes.ToDictionary(ct => ct.Type, ct => ct.Id);
			var catalogBrandIdLookup = context.CatalogBrands.ToDictionary(ct => ct.Brand, ct => ct.Id);

			var fileContent = File.ReadAllLines(fileName)
				.Skip(1)
				.Select(i => i.Split(','))
				.Select(i => new CatalogItem()
				{
					CatalogTypeId = catalogTypeIdLookup[i[0]],
					CatalogBrandId = catalogBrandIdLookup[i[1]],        //catalogBrandIdLookup[i[0]],
					Description = i[2].Trim('"').Trim(),
					Name = i[3].Trim('"').Trim(),
					Price = Decimal.Parse(i[4].Trim('"').Trim(), System.Globalization.NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture),
					PictureFileName = i[5].Trim('"').Trim(),
					AvailableStock = string.IsNullOrEmpty(i[6]) ? 0 : int.Parse(i[6]),
					OnOrder = Convert.ToBoolean(i[7]),


				});

			return fileContent;
		}

		private void GetCatalogItemPictures(string contentPath, string picturePath)
		{
			picturePath ??= "pics";

			if(picturePath != null)
			{
				DirectoryInfo directory = new DirectoryInfo(picturePath);
				foreach(FileInfo file in directory.GetFiles())
				{
					file.Delete();
				}

				string zipFileCatalogItemPictures = Path.Combine(contentPath, "CatalogItems.zip");
				ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
			}
		}

	}
}
