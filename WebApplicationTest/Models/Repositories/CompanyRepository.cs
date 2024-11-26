using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Company company)
        {
            try
            {
                _context.Companies.Add(company);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool Delete(int companyID)
        {
            try
            {
                var company = GetCompanyByID(companyID);
                _context.Companies.Remove(company);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            var companies = _context.Companies.ToList();
            return companies;
        }

        public Company GetCompanyByID(int companyID)
        {
            var company = GetAllCompanies().SingleOrDefault(x => x.ID == companyID);
            return company;
        }

        public bool Update(Company company)
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

        //public List<Transaction> GetTransactions()
        //{
        //    return _context.Transactions.ToList();
        //}

        //public List<TransactionItem> GetTransactionItems()
        //{
        //    return _context.TransactionItems.Include(x => x.Product)
        //                                        .Include(x => x.Transaction).ToList();
        //}

        public List<Review> GetReviews()
        {
            return _context.Reviews.Include(x => x.User)
                                        .Include(x => x.Product).ToList();
        }
    }
}
