using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class ShoppingCartService
    {
        private readonly ShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(ShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }
        public bool AddItemToCart(int itemID, int quantity, int userID)
        {
            bool addedItem = _shoppingCartRepository.AddItemToCart(itemID, quantity, userID);
            return addedItem;
        }

        public bool ClearCart()
        {
            bool clearedCart = _shoppingCartRepository.ClearCart();
            return clearedCart;
        }

        public bool DeleteCartItem(int itemID)
        {
            bool deletedItem = _shoppingCartRepository.DeleteCartItem(itemID);
            return deletedItem;
        }

        public CartItem GetCartItemByID(int itemID)
        {
            CartItem item = _shoppingCartRepository.GetCartItemByID(itemID);
            return item;
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            var cartItems = _shoppingCartRepository.GetCartItems();
            return cartItems;
        }


        public bool UpdateCartItemQuantity(int itemID, int quantity)
        {
            CartItem item = _shoppingCartRepository.GetCartItemByID(itemID);
            item.Quantity = quantity;
            bool updatedQuantity = _shoppingCartRepository.UpdateCartItem(item);
            return updatedQuantity;
        }

        public double CalculateCartTotal()
        {
            double total = _shoppingCartRepository.CalculateCartTotal();
            return total;
        }
        public bool UpdateCartItemShippingOption(int itemID, double newShippingCost, string shippingOption)
        {
            CartItem item = GetCartItemByID(itemID);
            item.ShippingCost = newShippingCost;
            item.ShippingOption = shippingOption;
            bool updatedShippingCost = _shoppingCartRepository.UpdateCartItem(item);
            return updatedShippingCost;
        }

        public List<DeliveryOption> GetDeliveryOptions()
        {
            List<DeliveryOption> options = _shoppingCartRepository.GetDeliveryOptions();
            return options;
        }

        public int GetItemsBoughtQuantity()
        {
            IEnumerable<CartItem> cartItems = _shoppingCartRepository.GetCartItems();
            int itemsBought = cartItems.Sum(x => x.Quantity);
            return itemsBought;
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            var products = _shoppingCartRepository.GetPopularProducts();
            return products;
        }
    }
}
