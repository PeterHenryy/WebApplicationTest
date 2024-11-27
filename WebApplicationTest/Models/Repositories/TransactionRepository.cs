using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Transaction transaction)
        {
            try
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var transactions = _context.Transactions.ToList();
            return transactions;
        }

        public Transaction GetTransactionByID(int transactionID)
        {
            var transaction = GetAllTransactions().SingleOrDefault(x => x.ID == transactionID);
            return transaction;
        }

        public Product GetProductByID(int? productID)
        {
            var product = _context.Products.SingleOrDefault(x => x.ID == productID);
            return product;
        }

        public List<CreditCard> GetSpecificUserCards(int userID)
        {
            var userCards = _context.CreditCards.Where(x => x.UserID == userID).ToList();
            return userCards;
        }

        public IEnumerable<Refund> GetAllRefunds()
        {
            var refunds = _context.Refunds.ToList();
            return refunds;
        }

        public IEnumerable<Coupon> GetCoupons()
        {
            var coupons = _context.Coupons.ToList();
            return coupons;
        }

        public bool UpdateCouponQuantity(Coupon coupon)
        {
            try
            {
                coupon.Quantity--;
                if (coupon.Quantity == 0)
                {
                    _context.Coupons.Remove(coupon);
                }
                else
                {
                    _context.Coupons.Update(coupon);
                }
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool UpdateProductStock(Product product)
        {
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public Company GetCompanyByID(int? companyID)
        {
            Company company = _context.Companies.SingleOrDefault(x => x.ID == companyID);
            return company;
        }

        public bool UpdateCompany(Company company)
        {
            try
            {
                _context.Companies.Update(company);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool CreateTransactionItem(TransactionItem item)
        {
            try
            {
                _context.TransactionItems.Add(item);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }

        public List<TransactionItem> GetTransactionItems()
        {
            List<TransactionItem> transactionItems = _context.TransactionItems.Include(x => x.Product).ToList();
            return transactionItems;
        }
    }
}
