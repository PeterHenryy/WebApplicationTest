using WebApplicationTest.Helpers.Enums.Refunds;
using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class RefundService
    {
        private readonly RefundRepository _refundRepos;

        public RefundService(RefundRepository refundRepos)
        {
            _refundRepos = refundRepos;
        }

        public bool Create(Refund refund)
        {
            bool createdRefund = _refundRepos.Create(refund);
            return createdRefund;
        }

        public bool Update(Refund refund)
        {
            bool updatedRefund = _refundRepos.Update(refund);
            return updatedRefund;
        }

        public IEnumerable<Refund> GetCompanyRefunds(int companyID)
        {
            var companyRefunds = _refundRepos.GetCompanyRefunds(companyID);
            return companyRefunds;
        }

        public IEnumerable<Refund> GetUserRefunds(int userID)
        {
            var userRefunds = _refundRepos.GetUserRefunds(userID);
            return userRefunds;
        }

        public Refund GetRefundByID(int refundID)
        {
            Refund refund = _refundRepos.GetRefundByID(refundID);
            return refund;
        }
        public bool HasAdminAcceptedRefund(bool accepted, Refund refund)
        {
            if (accepted)
            {
                refund.Status = RefundStatus.Accepted.ToString();
                return true;
            }
            refund.Status = RefundStatus.Denied.ToString();
            return false;

        }

        public TransactionItem GetTransactionItem(int? transactionID, int? productID)
        {
            List<TransactionItem> transactionItems = _refundRepos.GetTransactionItems();
            TransactionItem transactionItem = transactionItems.SingleOrDefault(x => x.TransactionID == transactionID && x.ProductID == productID);
            return transactionItem;
        }
    }
}
