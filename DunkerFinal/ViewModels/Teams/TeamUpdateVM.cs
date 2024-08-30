namespace DunkerFinal.ViewModels.Teams
{
    public class TeamUpdateVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
