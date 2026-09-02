using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;
using WebApplicationTest.Models.ViewModels;
using WebApplicationTest.Helpers.Enums;
using WebApplicationTest.Helpers;
using System.Globalization;
using Microsoft.CodeAnalysis;

namespace WebApplicationTest.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly UserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly CouponService _couponService;
        private readonly CreditCardService _creditCardService;
        private readonly ProductService _productService;
        private readonly AppUser _currentUser;

        public TransactionController(TransactionService transactionService, UserService userService, UserManager<AppUser> userManager, ShoppingCartService shoppingCartService, CouponService couponService, CreditCardService creditCardService, ProductService productService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
            _creditCardService = creditCardService;
            _productService = productService;
            _currentUser = userService.GetCurrentUser();
        }

        public IActionResult UserTransactions(int userID)
        {
            var userTransactionsViewModel = new UserTransactionsViewModel();
            userTransactionsViewModel.UserTransactions = _transactionService.GetTransactionsByUserID(userID);
            userTransactionsViewModel.UserRefunds = _transactionService.GetAllUserRefunds(userID);
            return View(userTransactionsViewModel);
        }

        [HttpGet]
        public IActionResult Create(string couponCode = null)
        {
            var transactionViewModel = new TransactionViewModel();
            transactionViewModel.Transaction = new Transaction();
            Transaction currentTransaction = transactionViewModel.Transaction;
            IEnumerable<CartItem> cartItems = _shoppingCartService.GetCartItems();
            transactionViewModel.CartItems = cartItems;
            double cartTotal = _shoppingCartService.CalculateCartTotal();
            _transactionService.PopulateViewModel(transactionViewModel, _currentUser.Id);
            _transactionService.CalculateTransactionTotal(cartTotal, currentTransaction, cartItems);
            if (!String.IsNullOrEmpty(couponCode))
            {
                _couponService.ValidateCoupon(currentTransaction, cartItems, couponCode);
            }
            currentTransaction.Total += transactionViewModel.TransactionTax;
            return View(transactionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionViewModel transactionViewModel)
        {
            Transaction currentTransaction = transactionViewModel.Transaction;
            currentTransaction.UserID = _currentUser.Id;
            currentTransaction.ItemsBought = _shoppingCartService.GetItemsBoughtQuantity();
            transactionViewModel.UserCards = _transactionService.GetSpecificUserCards(_currentUser.Id);

            if (string.IsNullOrEmpty(currentTransaction.PaymentType))
            {
                return ReturnTransactionWithError(transactionViewModel, "You must select a payment method!");
            }
            if (currentTransaction.PaymentType == PaymentTypes.RewardPoints.ToString())
            {
                bool validRewardPoints = _transactionService.ValidatePointsForTransaction(_currentUser.UserRewardPoints, currentTransaction.Total);
                if (!validRewardPoints) return ReturnTransactionWithError(transactionViewModel, "You do not have enough reward points for this purchase!");
            }
            if (currentTransaction.PaymentType == PaymentTypes.CreditCard.ToString() && transactionViewModel.ChosenCardID == null)
            {
                var validationError = _creditCardService.ValidateCreditCard(transactionViewModel.UserNewCard, currentTransaction.TransactionDate);
                if (validationError != null) return ReturnTransactionWithError(transactionViewModel, validationError);
                transactionViewModel.UserNewCard.UserID = _currentUser.Id;
                _creditCardService.Create(transactionViewModel.UserNewCard);
            }
            bool createdTransaction = _transactionService.Create(currentTransaction);
            if (createdTransaction)
            {
                if (currentTransaction.CouponCode != null)
                {
                    bool decreasedCoupon = _couponService.DecreaseCouponQuantity(currentTransaction.CouponCode, _shoppingCartService.GetCartItems());
                }
                IEnumerable<CartItem> cartItems = _shoppingCartService.GetCartItems();
                for (int i = 0; i < cartItems.Count(); i++)
                {
                    CartItem cartItem = cartItems.ElementAt(i);
                    double itemCost = cartItem.Quantity * cartItem.Product.Price;
                    _transactionService.UpdateProductStock(cartItem.ProductID, cartItem.Quantity);
                    _transactionService.UpdateCompanyProperties(cartItem.Product.CompanyID, itemCost, cartItem.Quantity);
                    _transactionService.CreateTransactionItem(cartItem, currentTransaction.ID);
                }
                bool clearedCart = _shoppingCartService.ClearCart();
                
                _transactionService.UpdateUserRewardPoints(currentTransaction.Total, _currentUser, currentTransaction.PaymentType == PaymentTypes.RewardPoints.ToString());
                await _userManager.UpdateAsync(_currentUser);
                return RedirectToAction("UserTransactions", "Transaction", new { userID = _currentUser.Id });
            }
            return View(transactionViewModel);
        }

        public IActionResult ReturnTransactionWithError(TransactionViewModel transactionViewModel, string message)
        {
            _transactionService.PopulateViewModel(transactionViewModel, _currentUser.Id);
            ModelState.Clear();
            ModelState.AddModelError("", message);
            return View("Create", transactionViewModel);
        }

        [HttpGet]
        public IActionResult ValidateCoupon(string couponCode, Transaction transaction = null)
        {
            double cartTotal = _shoppingCartService.CalculateCartTotal();
            transaction.Total = cartTotal;
            IEnumerable<CartItem> cartItems = _shoppingCartService.GetCartItems();
            CouponValidator validatedCoupon = _couponService.ValidateCoupon(transaction, cartItems, couponCode);

            return Json(validatedCoupon);
        }

        public IActionResult TransactionItems(int transactionID)
        {
            List<TransactionItem> transactionItems = _transactionService.GetTransactionItems(transactionID);
            var transaction = _transactionService.GetTransactionByID(transactionID);
            var transactionItemsVM = new TransactionItemsViewModel();
            transactionItemsVM.Transactionitems = transactionItems;
            transactionItemsVM.TransactionTotal = "$" + transaction.Total.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            transactionItemsVM.Refunds = _transactionService.GetAllUserRefunds(_currentUser.Id);
            transactionItemsVM.TransactionQuantityBought = transaction.ItemsBought;
            if (!String.IsNullOrEmpty(transaction.CouponCode))
            {
                transactionItemsVM.Discount = _transactionService.GetCouponDiscount(transaction.CouponPercentage, transaction.Total);
            }
            transactionItemsVM.Reviews = _productService.GetReviews();
            transactionItemsVM.CurrentUser = _currentUser;
            return View(transactionItemsVM);
        }

        [HttpGet]
        public IActionResult CheckRewardPoints(double transactionTotal)
        {
            bool result = _transactionService.ValidatePointsForTransaction(_currentUser.UserRewardPoints, transactionTotal);
            return Json(new { success = result });
        }
    }
}
