using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;
using WebApplicationTest.Models.ViewModels;

namespace WebApplicationTest.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly IBlobService _blobService;
        private readonly AppUser _user;

        public ProductController(ProductService productService, UserService userService, IBlobService blobService)
        {
            _productService = productService;
            _userService = userService;
            _blobService = blobService;
            _user = userService.GetCurrentUser();
        }

        public IActionResult Index()
        {
            var productIndexViewModel = new ProductIndexViewModel();
            productIndexViewModel.Products = _productService.GetAllProducts().ToList();
            productIndexViewModel.User = _user;
            var reviews = _productService.GetReviews();
            return View(productIndexViewModel);
        }

        public IActionResult ProductsDisplay(string filterOption = "", int optionIdentify = 0)
        {
            var productDisplayViewModel = new ProductDisplayViewModel();
            productDisplayViewModel.Companies = _productService.GetAllCompanies();
            productDisplayViewModel.Categories = _productService.GetAllCategories();
            int maxProductsPerPage = 12;
            productDisplayViewModel.Products = String.IsNullOrEmpty(filterOption) ? _productService.GetAllProducts().Take(maxProductsPerPage)
                                                                                     : _productService.GetFilteredProducts(filterOption, optionIdentify);
            double numberOfPages = (double)_productService.GetAllProducts().Count() / maxProductsPerPage;
            int roundedPages = (int)Math.Ceiling(numberOfPages);
            ViewBag.NumberOfPages = roundedPages;
            ViewBag.CurrentPageNumber = optionIdentify == 0 ? 1 : optionIdentify;
            productDisplayViewModel.User = _user;
            return View(productDisplayViewModel);
        }

        public IActionResult Delete(int productID)
        {
            _productService.Delete(productID);
            return RedirectToAction("CompanyProducts", "Product");
        }

        [HttpGet]
        public IActionResult Update(int productID)
        {
            Product product = _productService.GetProductByID(productID);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            await HandleBlob(product);
            _productService.CheckProductStockChange(product.ID, product.Stock);
            bool updatedProduct = _productService.Update(product);
            //_productService.HandleProductImages(product, files);
            if (updatedProduct)
            {
                return RedirectToAction("CompanyProducts", "Product");
            }
            return View(product);
        }

        public async Task HandleBlob(Product product)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                //product.Image = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                bool uploadedBlob = await _blobService.UploadBlob(files[0].FileName, files[0], new Blob());
                product.Image = _blobService.GetBlob(files[0].FileName);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductCreateViewModel createViewModel = new ProductCreateViewModel();
            createViewModel.Categories = _productService.GetAllCategories();
            createViewModel.User = _user;
            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel productVM)
        {
            var files = HttpContext.Request.Form.Files;
            productVM.Product.UserID = _user.Id;
            await HandleBlob(productVM.Product);
            bool createdProduct = _productService.Create(productVM.Product);
            if (createdProduct)
            {
                bool updatedStock = _productService.UpdateCompanyProductStock(productVM.Product.CompanyID, productVM.Product.Stock, "increase");
                return RedirectToAction("CompanyProducts", "Product");
            }

            return View(productVM.Product);
        }

        public IActionResult Details(int productID, int pageNumber)
        {
            Product product = _productService.GetProductByID(productID);
            product.AverageRating = _productService.CalculateProductAverageRating(productID);
            var detailsViewModel = new ProductDetailsViewModel();
            if (_user != null)
            {
                detailsViewModel.HasUserBoughtProduct = _productService.HasUserBoughtProduct(productID, _user.Id);
                detailsViewModel.CurrentUser = _user;
                detailsViewModel.HasUserReviewedProduct = _productService.HasUserReviewedProduct(productID, _user.Id);
            }
            detailsViewModel.Product = product;
            int maxReviewsPerPage = 5;
            detailsViewModel.Reviews = pageNumber == 0 ? _productService.GetReviewsOfSpecificProduct(productID).Take(maxReviewsPerPage).ToList()
                                                         : _productService.FilterReviews(pageNumber).ToList();
            double numberOfPages = (double)_productService.GetReviewsOfSpecificProduct(productID).Count() / maxReviewsPerPage;
            int roundedPages = (int)Math.Ceiling(numberOfPages);
            ViewBag.NumberOfPages = roundedPages;
            ViewBag.CurrentPageNumber = pageNumber == 0 ? 1 : pageNumber;
            detailsViewModel.Comments = _productService.GetAllComments().ToList();
            detailsViewModel.Likes = _productService.GetLikes().ToList();
            detailsViewModel.Dislikes = _productService.GetDislikes().ToList();
            detailsViewModel.ProductID = productID;
            detailsViewModel.ProductImages = _productService.GetProductImages(productID);
            detailsViewModel.ProductSales = _productService.GetProductSales(productID);
            return View(detailsViewModel);
        }

        public IActionResult CompanyProducts(string filterOption = "", int optionIdentify = 0)
        {
            var companyProductsDisplay = new ProductDisplayViewModel();
            companyProductsDisplay.Categories = _productService.GetAllCategories();
            int maxProductsPerPage = 12;
            companyProductsDisplay.Products = String.IsNullOrEmpty(filterOption) ? _productService.GetCompanyProducts(_user.CompanyID).Take(maxProductsPerPage)
                                                                                    : _productService.GetFilteredProducts(filterOption, optionIdentify, _user.CompanyID);
            double numberOfPages = (double)_productService.GetCompanyProducts(_user.CompanyID).Count() / maxProductsPerPage;
            int roundedPages = (int)Math.Ceiling(numberOfPages);
            ViewBag.NumberOfPages = roundedPages;
            ViewBag.CurrentPageNumber = optionIdentify == 0 ? 1 : optionIdentify;
            return View(companyProductsDisplay);
        }

        public IActionResult ManageProductArchiving(int productID, int option)
        {
            _productService.ManageProductArchiving(productID, option);
            return RedirectToAction("CompanyProducts", "Product");
        }

        
        
    }
}
