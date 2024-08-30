using Domain.Common;

namespace Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsMain { get; set; } = false;
        public bool IsHover { get; set; } = false;
    }
}
