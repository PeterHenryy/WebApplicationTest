namespace WebApplicationTest.Models.IRepositories
{
    public interface IRefundRepository
    {
        bool Create(Refund refund);
        bool Update(Refund refund);
        IEnumerable<Refund> GetUserRefunds(int userID);
        IEnumerable<Refund> GetCompanyRefunds(int companyID);
        Refund GetRefundByID(int refundID);
    }
}
