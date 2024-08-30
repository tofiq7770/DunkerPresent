using Domain.Entities;

namespace DunkerFinal.ViewModels.Header
{
    public class HeaderVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public int BasketCount { get; set; }
        public IEnumerable<BasketProduct> Baskets { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
