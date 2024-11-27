namespace WebApplicationTest.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public List<DeliveryOption> DeliveryOptions { get; set; }
        public bool UserHasCreditCard { get; set; }
    }
}
