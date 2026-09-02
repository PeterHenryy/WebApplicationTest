using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;
using System.Globalization;
using WebApplicationTest.Models.ViewModels;

namespace WebApplicationTest.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepos;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly CouponService _couponService;

        public TransactionService(TransactionRepository transactionRepos, ShoppingCartService shoppingCartService, CouponService couponService)
        {
            _transactionRepos = transactionRepos;
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
        }

        public bool Create(Transaction transaction)
        {
            var createdTransaction = _transactionRepos.Create(transaction);
            return createdTransaction;
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var transactions = _transactionRepos.GetAllTransactions();
            return transactions;
        }

        public Transaction GetTransactionByID(int transactionID)
        {
            var transaction = GetAllTransactions().SingleOrDefault(x => x.ID == transactionID);
            return transaction;
        }

        public Product GetProductByID(int? productID)
        {
            var product = _transactionRepos.GetProductByID(productID);
            return product;
        }

        public void CalculateTransactionTotal(double cartTotal, Transaction currentTransaction, IEnumerable<CartItem> cartItems)
        {
            currentTransaction.Total = cartTotal;
            foreach (var item in cartItems)
            {
                currentTransaction.Total += item.ShippingCost;
            }
            currentTransaction.Total = Math.Round((currentTransaction.Total * 100)) / 100;
        }

        public List<CreditCard> GetSpecificUserCards(int userID)
        {
            var userCards = _transactionRepos.GetSpecificUserCards(userID);
            return userCards;
        }

        public IEnumerable<Transaction> GetTransactionsByUserID(int userID)
        {
            var userTransactions = GetAllTransactions().Where(x => x.UserID == userID);
            return userTransactions;
        }

        public IEnumerable<Refund> GetAllUserRefunds(int userID)
        {
            var refunds = _transactionRepos.GetAllRefunds().Where(x => x.UserID == userID);
            return refunds;
        }



        public bool UpdateProductStock(int? productID, int quantity)
        {
            Product product = _transactionRepos.GetProductByID(productID);
            product.Stock -= quantity;
            bool updatedProduct = _transactionRepos.UpdateProductStock(product);
            return updatedProduct;
        }

        public bool UpdateCompanyProperties(int? companyID, double productTotal, int quantityBought)
        {
            Company company = _transactionRepos.GetCompanyByID(companyID);
            company.Revenue += productTotal;
            company.ProductsInStock -= quantityBought;
            company.TotalSales += quantityBought;
            bool updatedCompany = _transactionRepos.UpdateCompany(company);
            return updatedCompany;
        }

        public void CreateTransactionItem(CartItem cartItem, int transactionID)
        {
            TransactionItem transactionItem = new TransactionItem
            {
                Quantity = cartItem.Quantity,
                TransactionID = transactionID,
                ProductID = cartItem.ProductID,
                ShippingCost = cartItem.ShippingCost,
                ShippingOption = cartItem.ShippingOption
            };
            bool createdItem = _transactionRepos.CreateTransactionItem(transactionItem);
        }

        public double CalculateTransactionTax(double cartItemsTotal)
        {
            double taxPercentage = 8;
            double tax = Math.Round((cartItemsTotal * (taxPercentage / 100)) * 100) / 100;
            return tax;
        }


        public List<Category> GetCategories()
        {
            List<Category> categories = _transactionRepos.GetCategories();
            return categories;
        }

        public List<TransactionItem> GetTransactionItems(int transactionID)
        {
            List<TransactionItem> transactionItems = _transactionRepos.GetTransactionItems().Where(x => x.TransactionID == transactionID).ToList();
            return transactionItems;
        }
        public void UpdateUserRewardPoints(double transactionTotal, AppUser _currentUser, bool paidWithPoints)
        {
            if (paidWithPoints)
            {
                _currentUser.UserRewardPoints -= transactionTotal * 5;
            }
            else
            {
                double rewardPoints = transactionTotal / 2;
                _currentUser.UserRewardPoints += rewardPoints;
            }
        }
        public bool ValidatePointsForTransaction(double userRewardPoints, double transactionTotal)
        {
            return userRewardPoints >= transactionTotal * 5;
        }

        public string GetCouponDiscount(double couponPercentage, double transactionTotal )
        {
            var transactionDiscount = transactionTotal * (couponPercentage / 100);
            var roundedDiscount = Math.Round(transactionDiscount, 2);
            var formattedDiscount = roundedDiscount.ToString("F2", CultureInfo.InvariantCulture);
            return formattedDiscount;
        }

        public void PopulateViewModel(TransactionViewModel transactionViewModel, int userID)
        {
            var cartTotal = _shoppingCartService.CalculateCartTotal();
            transactionViewModel.UserCards = GetSpecificUserCards(userID);
            transactionViewModel.Categories = GetCategories();
            transactionViewModel.ItemsBought = _shoppingCartService.GetItemsBoughtQuantity();
            transactionViewModel.TransactionTax = CalculateTransactionTax(cartTotal);
            transactionViewModel.CartItems = _shoppingCartService.GetCartItems();
        }

       
    }
}
