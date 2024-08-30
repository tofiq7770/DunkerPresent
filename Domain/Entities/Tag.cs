using Domain.Common;
namespace Domain.Entities
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
