using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public List<DeliveryOption> DeliveryOptions { get; set; }
        public bool UserHasCreditCard { get; set; }
        public IEnumerable<Product> PopularProducts { get; set; }
        public AppUser User { get; set; }

    }
}
