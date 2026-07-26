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
        private readonly AppUser _currentUser;

        public TransactionController(TransactionService transactionService, UserService userService, UserManager<AppUser> userManager, ShoppingCartService shoppingCartService, CouponService couponService, CreditCardService creditCardService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
            _creditCardService = creditCardService;
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
            transactionViewModel.UserCards = _transactionService.GetSpecificUserCards(_currentUser.Id);
            if (!_currentUser.HasCreditCard)
            {
                return RedirectToAction("Create", "CreditCard", new {redirectToTransaction = true});
            }
            Transaction currentTransaction = transactionViewModel.Transaction;
            double cartTotal = _shoppingCartService.CalculateCartTotal();
            IEnumerable<CartItem> cartItems = _shoppingCartService.GetCartItems();
            _transactionService.CalculateTransactionTotal(cartTotal, currentTransaction, cartItems);
            if (!String.IsNullOrEmpty(couponCode))
            {
                _couponService.ValidateCoupon(currentTransaction, cartItems, couponCode);
            }
            transactionViewModel.CartItems = cartItems;
            transactionViewModel.Categories = _transactionService.GetCategories();
            transactionViewModel.ItemsBought = _shoppingCartService.GetItemsBoughtQuantity();
            transactionViewModel.TransactionTax = _transactionService.CalculateTransactionTax(cartTotal);
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
            if (transactionViewModel.UserNewCard != null)
            {
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
                bool validRewardPoints = false;
                if (currentTransaction.PaymentType == PaymentTypes.RewardPoints.ToString())
                {
                    validRewardPoints = _transactionService.ValidatePointsForTransaction(_currentUser.UserRewardPoints, currentTransaction.Total);
                }
                _transactionService.UpdateUserRewardPoints(currentTransaction.Total, _currentUser, validRewardPoints);
                await _userManager.UpdateAsync(_currentUser);
                return RedirectToAction("UserTransactions", "Transaction", new { userID = _currentUser.Id });
            }
            return View(transactionViewModel);
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
