using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;
using WebApplicationTest.Models.ViewModels;

namespace WebApplicationTest.Controllers
{
    public class CouponController : Controller
    {
        private readonly CouponService _couponService;
        private readonly UserService _userService;
        private readonly AppUser _user;

        public CouponController(CouponService couponService, UserService userService)
        {
            _couponService = couponService;
            _userService = userService;
            _user = userService.GetCurrentUser();

        }

        public IActionResult CompanyCoupons(int companyID)
        {
            var companyCoupons = _couponService.GetCompanyCoupons(companyID);
            return View(companyCoupons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var couponCreateViewModel = new CouponCreateViewModel();
            couponCreateViewModel.Products = _couponService.GetCompanyProducts(_user.CompanyID).ToList();
            couponCreateViewModel.Categories = _couponService.GetCompanyCategories(_user.CompanyID).ToList();
            return View(couponCreateViewModel);
        }

        [HttpPost]
        public IActionResult Create(Coupon coupon)
        {
            coupon.CompanyID = _user.CompanyID;
            bool createdCoupon = _couponService.Create(coupon);
            if (createdCoupon)
            {
                return RedirectToAction("CompanyCoupons", "Coupon", new { companyID = coupon.CompanyID });
            }
            return View(coupon);
        }

        public IActionResult Delete(int couponID)
        {
            bool deletedCoupon = _couponService.Delete(couponID);
            return RedirectToAction("CompanyCoupons", "Coupon", new { companyID = _user.CompanyID });
        }
    }
}
