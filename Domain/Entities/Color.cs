using Domain.Common;

namespace Domain.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
    }
}
