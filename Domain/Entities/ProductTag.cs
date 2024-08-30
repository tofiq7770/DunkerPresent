using Domain.Common;
namespace Domain.Entities
{
    public class ProductTag:BaseEntity
    {
        public int ProductId { get; set; }
        public int TagId { get; set; }

        public Product Product { get; set; }
        public Tag Tag { get; set; }

    }
}
