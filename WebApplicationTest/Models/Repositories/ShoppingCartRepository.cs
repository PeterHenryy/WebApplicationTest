using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models.IRepositories;
using WebApplicationTest.Services;

namespace WebApplicationTest.Models.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppUser _currentUser;

        public ShoppingCartRepository(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _currentUser = userService.GetCurrentUser();
        }
        public bool AddItemToCart(int itemID, int quantity, int userID)
        {
            if (userID == 0)
            {
                userID = _currentUser.Id;
            }
            ShoppingCart userCart = GetUserCart(userID);
            if (userCart == null)
            {
                userCart = CreateUserCart(userID);
            }
            CartItem cartItem = GetCartItemByID(itemID, userID);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                bool updatedItem = UpdateCartItem(cartItem);
                return updatedItem;
            }
            else
            {
                cartItem = new CartItem
                {
                    Quantity = quantity,
                    CartID = userCart.ID,
                    ProductID = itemID,
                    ShippingOption = "Free Shipping"
                };
                try
                {
                    _context.CartItems.Add(cartItem);
                    _context.SaveChanges();
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
        public bool DeleteCart()
        {
            try
            {
                ShoppingCart userCart = GetUserCart(_currentUser.Id);
                _context.ShoppingCarts.Remove(userCart);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool ClearCart()
        {
            try
            {
                IEnumerable<CartItem> cartItems = GetCartItems();
                _context.CartItems.RemoveRange(cartItems);
                DeleteCart();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool DeleteCartItem(int itemID)
        {
            try
            {
                CartItem item = GetCartItemByID(itemID, _currentUser.Id);
                _context.CartItems.Remove(item);
                _context.SaveChanges();
                IEnumerable<CartItem> cartItems = GetCartItems();
                if (cartItems.Count() == 0)
                {
                    DeleteCart();
                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public CartItem GetCartItemByID(int itemID, int userID = 0)
        {
            if (userID == 0)
            {
                userID = _currentUser.Id;
            }
            ShoppingCart userCart = GetUserCart(userID);
            CartItem item = _context.CartItems.Where(x => x.CartID == userCart.ID).SingleOrDefault(x => x.ProductID == itemID);
            return item;
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            ShoppingCart userCart = GetUserCart(_currentUser.Id);
            IEnumerable<CartItem> cartItems = null;
            if (userCart != null)
            {
                cartItems = _context.CartItems.Where(x => x.CartID == userCart.ID).Include(x => x.Product);
            }
            return cartItems;
        }

        public ShoppingCart GetUserCart(int userID)
        {
            ShoppingCart userCart = _context.ShoppingCarts.SingleOrDefault(x => x.UserID == userID);
            return userCart;
        }

        public ShoppingCart CreateUserCart(int userID)
        {
            ShoppingCart userCart = new ShoppingCart
            {
                UserID = userID,
            };
            _context.ShoppingCarts.Add(userCart);
            _context.SaveChanges();
            userCart = GetUserCart(userID);
            return userCart;
        }

        public double CalculateCartTotal()
        {
            IEnumerable<CartItem> cartItems = GetCartItems();
            double total = (cartItems == null) ? 0 : cartItems.Sum(x => x.Product.Price * x.Quantity);
            return (total * 100) / 100;
        }

        public bool UpdateCartItem(CartItem item)
        {
            try
            {
                _context.CartItems.Update(item);
                _context.SaveChanges();
                return true;

            }
            catch (System.Exception)
            {

                return false;
            }

        }

        public List<DeliveryOption> GetDeliveryOptions()
        {
            List<DeliveryOption> options = _context.DeliveryOptions.ToList();
            return options;
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            var products = _context.Products.Include(x => x.Category).OrderByDescending(x => x.AverageRating).Take(4);
            return products;
        }
    }
}
