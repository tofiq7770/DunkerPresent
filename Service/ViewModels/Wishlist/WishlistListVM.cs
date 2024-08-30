namespace Service.ViewModels.Wishlist
{
    public class WishlistListVM
    {

        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
