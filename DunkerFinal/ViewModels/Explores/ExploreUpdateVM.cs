namespace DunkerFinal.ViewModels.Explores
{
    public class ExploreUpdateVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
