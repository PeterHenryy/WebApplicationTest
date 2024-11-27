namespace WebApplicationTest.Models.IRepositories
{
    public interface ICouponRepository
    {
        bool Create(Coupon coupon);
        bool Delete(int couponID);
        Coupon GetCouponByID(int couponID);
        IEnumerable<Coupon> GetCompanyCoupons(int? companyID);
        bool Update(Coupon coupon);
    }
}
