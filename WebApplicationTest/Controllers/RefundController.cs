using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;
using WebApplicationTest.Models.ViewModels;
using WebApplicationTest.Helpers.Enums;

namespace WebApplicationTest.Controllers
{
    public class RefundController : Controller
    {
        private readonly RefundService _refundService;
        private readonly UserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppUser _currentUser;

        public RefundController(RefundService refundService, UserService userService, UserManager<AppUser> userManager)
        {
            _refundService = refundService;
            _userService = userService;
            _userManager = userManager;
            _currentUser = userService.GetCurrentUser();
        }

        public IActionResult UserRefunds()
        {
            var userRefunds = _refundService.GetUserRefunds(_currentUser.Id);
            return View(userRefunds);
        }

        public IActionResult CompanyRefunds()
        {
            var companyRefunds = _refundService.GetCompanyRefunds(_currentUser.CompanyID);
            return View(companyRefunds);
        }

        [HttpPost]
        public IActionResult Create(TransactionItemsViewModel refundForm)
        {
            refundForm.Refund.UserID = _currentUser.Id;
            bool createdRefund = _refundService.Create(refundForm.Refund);
            return RedirectToAction("UserRefunds", "Refund");
        }

        public async Task<IActionResult> UpdateRefundStatus(int refundID, bool accepted, string paymentType)
        {
            Refund refund = _refundService.GetRefundByID(refundID);
            TransactionItem transactionItem = _refundService.GetTransactionItem(refund.TransactionID, refund.ProductID);
            double transactionItemTotal = transactionItem.Quantity * transactionItem.Product.Price;
            bool refundAccepted = _refundService.HasAdminAcceptedRefund(accepted, refund);
            if (refundAccepted)
            {
                await RefundUserRewardPoints(transactionItemTotal, paymentType, refund.UserID);
            }
            _refundService.Update(refund);
            return RedirectToAction("CompanyRefunds", "Refund");
        }

        public async Task RefundUserRewardPoints(double transactionItemTotal, string paymentType, int? userID)
        {
            AppUser user = _userManager.FindByIdAsync(userID.ToString()).Result;
            if (paymentType == PaymentTypes.CreditCard.ToString())
            {
                user.UserRewardPoints -= transactionItemTotal / 2;
            }
            else
            {
                user.UserRewardPoints += transactionItemTotal * 5;
            }
            await _userManager.UpdateAsync(user);
        }
    }
}
