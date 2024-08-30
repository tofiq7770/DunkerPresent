using Domain.Entities;
using DunkerFinal.Areas.Admin.Class;

namespace DunkerFinal.ViewModels.Products
{
    public class ProductsListVM
    {
        public PagedList<Product> Products { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

}
