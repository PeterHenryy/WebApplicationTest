using Microsoft.CodeAnalysis;
using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class TransactionItemsViewModel
    {
        public List<TransactionItem> Transactionitems { get; set; }
        public IEnumerable<Refund> Refunds { get; set; }
        public string TransactionTotal { get; set; }
        public int TransactionQuantityBought { get; set; }
        public Refund Refund { get; set; }
        public bool HasRequestedRefund(int transactionItemID)
        {
            bool refundExists = Refunds.Any(x => x.TransactionItemID == transactionItemID);
            return refundExists;
        }
        public string Discount { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public bool HasUserReviewedProduct(int? productID, int userID)
        {
            return Reviews.Any(x => x.ProductID == productID && x.UserID == userID);
        }
        public AppUser CurrentUser { get; set; }
        public Review Review { get; set; }



    }
}
