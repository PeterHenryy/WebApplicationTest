using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApplicationTest.Helpers;
using WebApplicationTest.Models;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models.ViewModels;
using WebApplicationTest.Services;

namespace WebApplicationTest.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        private readonly UserService _userService;
        private readonly CouponService _couponService;
        private readonly AppUser _currentUser;

        public ShoppingCartController(ShoppingCartService shoppingCartService, UserService userService, CouponService couponService)
        {
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _couponService = couponService;
            _currentUser = userService.GetCurrentUser();
        }

        public IActionResult DisplayCartItems()
        {
            var shoppingCartViewModel = new ShoppingCartViewModel();
            shoppingCartViewModel.CartItems = _shoppingCartService.GetCartItems();
            shoppingCartViewModel.DeliveryOptions = _shoppingCartService.GetDeliveryOptions();
            shoppingCartViewModel.UserHasCreditCard = _currentUser.HasCreditCard;
            shoppingCartViewModel.PopularProducts = _shoppingCartService.GetPopularProducts();
            shoppingCartViewModel.User = _currentUser;
            return View(shoppingCartViewModel);
        }
        [HttpPost]
        public void AddItemToCart(int itemID, int quantity, int userID)
        {
            bool addedItem = _shoppingCartService.AddItemToCart(itemID, quantity, userID);
        }

        public IActionResult ClearCart()
        {
            bool clearedCart = _shoppingCartService.ClearCart();
            return RedirectToAction("DisplayCartItems");
        }

        public IActionResult DeleteCart()
        {
            bool deletedCart = _shoppingCartService.ClearCart();
            return RedirectToAction("DisplayCartItems");
        }

        [HttpPost]
        public void UpdateCartItemQuantity(int itemID, int quantity)
        {
            bool updatedItem = _shoppingCartService.UpdateCartItemQuantity(itemID, quantity);
        }

        [HttpPost]
        public bool RemoveFromCart(int itemID, string couponCode = null)
        {
            bool removedItem = _shoppingCartService.DeleteCartItem(itemID);
            IEnumerable<CartItem> cartItems = _shoppingCartService.GetCartItems();
            if(removedItem && !string.IsNullOrEmpty(couponCode))
            {
                Transaction transaction = new Transaction();
                double cartTotal = _shoppingCartService.CalculateCartTotal();
                transaction.Total = cartTotal;
                CouponValidator validatedCoupon = _couponService.ValidateCoupon(transaction, cartItems, couponCode);
                return !validatedCoupon.CouponValid;
            }
            if (cartItems.Count() == 0)
            {
                DeleteCart();
            }
            return true;
        }

        [HttpPost]
        public void UpdateCartItemShippingOption(int itemID, string newShippingCost, string newShippingOption)
        {
            double shippingCost = double.Parse(newShippingCost, CultureInfo.GetCultureInfo("en-US"));
            bool updatedShippingCost = _shoppingCartService.UpdateCartItemShippingOption(itemID, shippingCost, newShippingOption);
        }
    }
}
