using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Helpers.Enums.Refunds;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class RefundRepository : IRefundRepository
    {
        private readonly ApplicationDbContext _context;

        public RefundRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Refund refund)
        {
            try
            {
                refund.Status = RefundStatus.Pending.ToString();
                _context.Refunds.Add(refund);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public IEnumerable<Refund> GetCompanyRefunds(int companyID)
        {
            var refunds = _context.Refunds.Include(x => x.Product)
                                                               .Include(x => x.Transaction)
                                                               .Include(x => x.User)
                                                               .Include(x => x.TransactionItem);

            var companyRefunds = refunds.Where(x => x.Product.CompanyID == companyID).ToList();
            return companyRefunds;
        }

        public Refund GetRefundByID(int refundID)
        {
            Refund refund = _context.Refunds.SingleOrDefault(x => x.ID == refundID);
            return refund;
        }

        public IEnumerable<Refund> GetUserRefunds(int userID)
        {
            var userRefunds = _context.Refunds.Include(x => x.Product)
                                                                         .Include(x => x.Transaction)
                                                                         .Include(x => x.TransactionItem)
                                                                         .Where(x => x.UserID == userID).ToList();
            return userRefunds;
        }

        public bool Update(Refund refund)
        {
            try
            {
                _context.Refunds.Update(refund);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public List<TransactionItem> GetTransactionItems()
        {
            List<TransactionItem> transactionItems = _context.TransactionItems.Include(x => x.Product).ToList();
            return transactionItems;
        }
    }
}
