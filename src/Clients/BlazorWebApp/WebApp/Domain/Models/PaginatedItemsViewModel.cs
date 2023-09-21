using System.Collections.Generic;

namespace WebApp.Domain.Models
{
    public class PaginatedItemsViewModel<TEntity> where TEntity : class
    {
        public int PagaIndex { get; set; }

        public int PageSize { get; set; }

        public long Count { get; set; }

        public IEnumerable<TEntity> Data { get; set; }

        public PaginatedItemsViewModel(int pagaIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PagaIndex = pagaIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public PaginatedItemsViewModel()
        {
            
        }

    }
}
