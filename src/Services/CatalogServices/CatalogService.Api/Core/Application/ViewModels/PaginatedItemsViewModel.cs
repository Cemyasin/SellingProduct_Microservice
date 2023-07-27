﻿using System.Collections.Generic;

namespace CatalogService.Api.Core.Application.ViewModels
{
	public class PaginatedItemsViewModel<TEntity> where TEntity : class
	{
		public int							PagaIndex				{ get; private set; }

		public int							PageSize				{ get; private set; }
																	
		public long							Count					{ get; private set; }
																	
		public IEnumerable<TEntity>			Data					{ get; private set; }

		public PaginatedItemsViewModel(int pagaIndex, int pageSize, long count, IEnumerable<TEntity> data)
		{
			PagaIndex = pagaIndex;
			PageSize  = pageSize;
			Count     = count;
			Data      = data;
		}
	}
}