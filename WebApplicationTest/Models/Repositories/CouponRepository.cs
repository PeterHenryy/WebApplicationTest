using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Coupon coupon)
        {
            try
            {
                _context.Coupons.Add(coupon);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool Delete(int couponID)
        {
            try
            {
                Coupon coupon = GetCouponByID(couponID);
                _context.Coupons.Remove(coupon);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public IEnumerable<Coupon> GetCompanyCoupons(int? companyID)
        {
            var companyCoupons = _context.Coupons.Where(x => x.CompanyID == companyID)
                                                   .Include(x => x.Product)
                                                   .Include(x => x.Category).ToList();
            return companyCoupons;
        }

        public Coupon GetCouponByID(int couponID)
        {
            Coupon coupon = _context.Coupons.SingleOrDefault(x => x.ID == couponID);
            return coupon;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _context.Products.ToList();
            return products;
        }

        public bool Update(Coupon coupon)
        {
            try
            {
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }


    }
}
