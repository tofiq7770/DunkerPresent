using Domain.Common;

namespace Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
