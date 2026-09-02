using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;
using WebApplicationTest.Helpers;

namespace WebApplicationTest.Services
{
    public class CouponService
    {
        private readonly CouponRepository _couponRepos;

        public CouponService(CouponRepository couponRepos)
        {
            _couponRepos = couponRepos;
        }

        public bool Create(Coupon coupon)
        {
            bool createdCoupon = _couponRepos.Create(coupon);
            return createdCoupon;
        }

        public bool Delete(int couponID)
        {
            bool deletedCoupon = _couponRepos.Delete(couponID);
            return deletedCoupon;
        }

        public IEnumerable<Coupon> GetCompanyCoupons(int? companyID)
        {
            var companyCoupons = _couponRepos.GetCompanyCoupons(companyID).ToList();
            return companyCoupons;
        }

        public IEnumerable<Category> GetCompanyCategories(int companyID)
        {
            var companyProducts = _couponRepos.GetProducts().Where(x => x.CompanyID == companyID);
            var categories = _couponRepos.GetCategories();
            List<Category> companyCategories = new List<Category>();
            foreach (var category in categories)
            {
                if (companyProducts.Any(x => x.CategoryID == category.ID))
                {
                    companyCategories.Add(category);
                }
            }
            return companyCategories;
        }

        public IEnumerable<Product> GetCompanyProducts(int? companyID)
        {
            var companyProducts = _couponRepos.GetProducts().Where(x => x.CompanyID == companyID);
            return companyProducts;
        }
        public Coupon GetCoupon(string code, int? companyID)
        {
            Coupon coupon = GetCompanyCoupons(companyID).SingleOrDefault(x => x.Code == code);
            return coupon;
        }

        public CouponValidator ValidateCoupon(Transaction transaction, IEnumerable<CartItem> cartItems, string couponCode)
        {
            CouponValidator couponValidator = new CouponValidator();

            foreach (var item in cartItems)
            {
                Coupon coupon = GetCoupon(couponCode, item.Product.CompanyID);
                bool isCouponValid = couponValidator.Validate(coupon, item.Product);
                if (isCouponValid)
                {
                    transaction.CouponCode = couponCode;
                    transaction.CouponPercentage = coupon.DiscountPercentage;
                    if(coupon.ProductID != 0)
                    {
                        transaction.Total -= (item.Product.Price * (coupon.DiscountPercentage / 100)) * 100 / 100;
                    }
                    if (coupon.CategoryID != 0)
                    {
                        var productsCategoryTotal = cartItems.Where(x => x.Product.CategoryID == coupon.CategoryID).Sum(x => x.Product.Price);
                        transaction.Total -= (productsCategoryTotal * (coupon.DiscountPercentage / 100)) * 100 / 100;
                    }
                    couponValidator.CouponValid = isCouponValid;
                    couponValidator.Coupon = coupon;
                    break;
                }
            }
            couponValidator.Total = transaction.Total;
            return couponValidator;
        }

        public bool DecreaseCouponQuantity(string couponCode, IEnumerable<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                Coupon coupon = GetCoupon(couponCode, item.Product.CompanyID);
                if (coupon != null)
                {
                    coupon.Quantity--;
                    UpdateCoupon(coupon);
                    return true;
                }
            }
            return false;
        }

        public bool UpdateCoupon(Coupon coupon)
        {
            bool updatedCoupon = _couponRepos.Update(coupon);
            return updatedCoupon;
        }
    }
}
