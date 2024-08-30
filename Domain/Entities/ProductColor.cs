using Domain.Common;

namespace Domain.Entities
{
    public class ProductColor : BaseEntity
    {

        public int ProductId { get; set; }
        public int ColorId { get; set; }

        public Product Product { get; set; }
        public Color Color { get; set; }
    }
}
