namespace DunkerFinal.ViewModels.Dashboard
{
    public class DashboardVM
    {
        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public int BrandCount { get; set; }
        public int TagCount { get; set; }
        public int ColorCount { get; set; }

        public List<string> ColorLabels { get; set; }
        public List<int> ProductColors { get; set; }
        public List<string> TagLabels { get; set; }
        public List<int> ProductTags { get; set; }

    }

}
