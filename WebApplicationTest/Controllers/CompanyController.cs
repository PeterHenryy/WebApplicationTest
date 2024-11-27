using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;
using WebApplicationTest.Models.ViewModels;

namespace WebApplicationTest.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;
        private readonly UserService _userService;
        private readonly AppUser _currentUser;

        public CompanyController(CompanyService companyService, UserService userService)
        {
            _companyService = companyService;
            _userService = userService;
            _currentUser = userService.GetCurrentUser();
        }

        public IActionResult Index()
        {
            var companies = _companyService.GetAllCompanies();
            return View(companies);
        }

        public IActionResult CompanyStats()
        {
            Company userCompany = _companyService.GetCompanyByID(_currentUser.CompanyID);
            var companyStatsViewModel = new CompanyStatsViewModel();
            companyStatsViewModel.Company = userCompany;
            companyStatsViewModel.YearCustomers = _companyService.GetCompanyYearCustomers(userCompany.ID);
            companyStatsViewModel.YearOrders = _companyService.GetCompanyYearOrders(userCompany.ID);
            companyStatsViewModel.YearRevenue = _companyService.GetCompanyYearRevenue(userCompany.ID);
            companyStatsViewModel.CompanyProductsReviews = _companyService.GetCompanyLatestReviews(userCompany.ID);
            companyStatsViewModel.CompanyPurchasedItems = _companyService.GetCompanyLatestPurchases(userCompany.ID);
            return View(companyStatsViewModel);
        }

        [HttpGet]
        public IActionResult Update(int companyID)
        {
            var company = _companyService.GetCompanyByID(companyID);
            return View(company);
        }

        [HttpPost]
        public IActionResult Update(Company company)
        {
            var updatedCompany = _companyService.Update(company);
            if (updatedCompany)
            {
                return RedirectToAction("Index", "Company");
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            var updatedCompany = _companyService.Create(company);
            if (updatedCompany)
            {
                return RedirectToAction("Index", "Company");
            }
            return View();
        }

        public IActionResult Delete(int companyID)
        {
            _companyService.Delete(companyID);
            return RedirectToAction("Index", "Company");
        }

        [HttpGet]
        public IActionResult GetCompanyRevenuesPerMonth()
        {
            var revenues = _companyService.GetCompanyRevenuesPerMonth(_currentUser.CompanyID);
            return Json(revenues);
        }

        public IActionResult CompanyProductReviews()
        {
            var companyProductReviews = _companyService.GetCompanyReviews(_currentUser.CompanyID);
            return View(companyProductReviews);
        }

        public IActionResult CompanyPurchasedProducts()
        {
            var companyPurchasedProducts = _companyService.GetCompanyTransactionItems(_currentUser.CompanyID);
            return View(companyPurchasedProducts);
        }
    }
}
