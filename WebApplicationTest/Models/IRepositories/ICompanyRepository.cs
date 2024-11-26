namespace WebApplicationTest.Models.IRepositories
{
    public interface ICompanyRepository
    {
        bool Create(Company company);
        bool Update(Company company);
        Company GetCompanyByID(int companyID);
        IEnumerable<Company> GetAllCompanies();
        bool Delete(int companyID);
    }
}
