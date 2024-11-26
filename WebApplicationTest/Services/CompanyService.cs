using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _companiesRepo;

        public CompanyService(CompanyRepository companiesRepo)
        {
            _companiesRepo = companiesRepo;
        }

        public bool Create(Company company)
        {
            var createdCompany = _companiesRepo.Create(company);
            return createdCompany;
        }

        public bool Delete(int companyID)
        {
            var deletedCompany = _companiesRepo.Delete(companyID);
            return deletedCompany;
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            var companies = _companiesRepo.GetAllCompanies();
            return companies;
        }

        public Company GetCompanyByID(int companyID)
        {
            var company = _companiesRepo.GetCompanyByID(companyID);
            return company;
        }

        public bool Update(Company company)
        {
            var updatedCompany = _companiesRepo.Update(company);
            return updatedCompany;
        }

        //public IEnumerable<TransactionItem> GetCompanyTransactionItems(int companyID)
        //{
        //    return _companiesRepo.GetTransactionItems().Where(x => x.Product.CompanyID == companyID);
        //}

        //public double GetCompanyYearRevenue(int companyID)
        //{
        //    IEnumerable<TransactionItem> companyTransactionItems = GetCompanyTransactionItems(companyID);
        //    var yearRevenue = GetCompanyYearTransactions(companyID).Sum(x => x.Product.Price * x.Quantity);
        //    return yearRevenue;
        //}

        //public IEnumerable<TransactionItem> GetCompanyYearTransactions(int companyID)
        //{
        //    return GetCompanyTransactionItems(companyID).Where(x => x.Transaction.TransactionDate.Year == DateTime.Now.Year);
        //}

        //public int GetCompanyYearCustomers(int companyID)
        //{
        //    var companyYearTransctions = GetCompanyYearTransactions(companyID);
        //    int customers = companyYearTransctions.GroupBy(x => x.Transaction.UserID).Count();
        //    return customers;
        //}

        //public int GetCompanyYearOrders(int companyID)
        //{
        //    int orders = GetCompanyYearTransactions(companyID).Count();
        //    return orders;
        //}


        //public List<double> GetCompanyRevenuesPerMonth(int companyID)
        //{
        //    IEnumerable<TransactionItem> companyYearTransactions = GetCompanyYearTransactions(companyID);
        //    List<double> revenuesPerMonth = new List<double>();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        double monthRevenue = 0;
        //        monthRevenue += companyYearTransactions.Where(x => x.Transaction.TransactionDate.Month == i)
        //                                                    .Sum(x => x.Product.Price * x.Quantity);
        //        revenuesPerMonth.Add(monthRevenue);
        //    }
        //    return revenuesPerMonth;
        //}

        public List<Review> GetCompanyLatestReviews(int companyID)
        {
            var reviews = GetCompanyReviews(companyID).OrderByDescending(x => x.Date)
                                                        .Take(4)
                                                        .ToList();

            return reviews;
        }

        //public List<TransactionItem> GetCompanyLatestPurchases(int companyID)
        //{
        //    var purchases = GetCompanyTransactionItems(companyID).OrderByDescending(x => x.Transaction.TransactionDate)
        //                                                            .Take(3)
        //                                                            .ToList();
        //    return purchases;
        //}

        public List<Review> GetCompanyReviews(int companyID)
        {
            var reviews = _companiesRepo.GetReviews()
                                            .Where(x => x.Product.CompanyID == companyID)
                                                .ToList();
            return reviews;
        }
    }
}
