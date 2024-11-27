namespace WebApplicationTest.Models.IRepositories
{
    public interface IShoppingCartRepository
    {
        bool DeleteCartItem(int itemID);
        bool AddItemToCart(int itemID, int quantity, int userID);
        bool ClearCart();
        bool DeleteCart();
        bool UpdateCartItem(CartItem item);
        IEnumerable<CartItem> GetCartItems();
        ShoppingCart GetUserCart(int userID);
        CartItem GetCartItemByID(int itemID, int userID);
        double CalculateCartTotal();
    }
}
