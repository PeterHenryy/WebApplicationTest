using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;
using NuGet.Protocol.Core.Types;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationTest.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepos;
        private readonly IWebHostEnvironment _environment;

        public ProductService(ProductRepository productRepos, IWebHostEnvironment environment)
        {
            _productRepos = productRepos;
            _environment = environment;
        }

        public bool Create(Product product)
        {
            bool createdProduct = _productRepos.Create(product);
            return createdProduct;
        }

        public bool Delete(int productID)
        {
            var deletedProduct = _productRepos.Delete(productID);
            return deletedProduct;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = _productRepos.GetAllProducts().Where(x => !x.Archived);
            return products;
        }

        public Product GetProductByID(int? productID)
        {
            var product = _productRepos.GetProductByID(productID);
            return product;
        }

        public bool Update(Product product)
        {
            var updatedProduct = _productRepos.Update(product);
            return updatedProduct;
        }

        public List<Category> GetAllCategories()
        {
            var categories = _productRepos.GetAllCategories();
            return categories;
        }

        public List<Company> GetAllCompanies()
        {
            var companies = _productRepos.GetAllCompanies();
            return companies;
        }

        public IEnumerable<Review> GetReviewsOfSpecificProduct(int productID)
        {
            var productReviews = _productRepos.GetReviews().Where(x => x.ProductID == productID).OrderByDescending(x => x.Date);
            return productReviews;
        }

        public IEnumerable<Review> GetReviews()
        {
            var reviews = _productRepos.GetReviews();
            return reviews;
        }

        public bool HasUserBoughtProduct(int productID, int userID)
        {
            var transactionItems = _productRepos.GetTransactionItems();
            bool hasBought = transactionItems.Any(x => x.Transaction.UserID == userID && x.ProductID == productID);
            return hasBought;
        }

        public IEnumerable<Comment> GetAllComments()
        {
            var comments = _productRepos.GetAllComments();
            return comments;
        }

        public IEnumerable<Like> GetLikes()
        {
            var likes = _productRepos.GetLikes();
            return likes;
        }

        public IEnumerable<Dislike> GetDislikes()
        {
            var dislikes = _productRepos.GetDislikes();
            return dislikes;
        }

        public bool HasUserReviewedProduct(int productID, int userID)
        {
            var userReview = GetReviewsOfSpecificProduct(productID).Where(x => x.UserID == userID);
            return userReview.Any();
        }

        public double CalculateProductAverageRating(int productID)
        {
            var productReviews = GetReviewsOfSpecificProduct(productID);
            if (productReviews.Any())
            {
                double rating = productReviews.Sum(x => x.Rating) / (double)productReviews.Count();
                double roundedRating = Math.Round(rating, 1);
                return roundedRating;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<Product> GetCompanyProducts(int companyID)
        {
            var companyProducts = _productRepos.GetAllProducts().Where(x => x.CompanyID == companyID);
            return companyProducts;
        }

        public void ManageProductArchiving(int productID, int option)
        {
            Product product = GetProductByID(productID);
            if (option == 1)
            {
                product.Archived = true;
            }
            else
            {
                product.Archived = false;
            }
            _productRepos.Update(product);
        }

        public bool UpdateCompanyProductStock(int? companyID, int quantity, string action)
        {
            Company company = _productRepos.GetCompanyByID(companyID);
            if (action == "increase")
            {
                company.ProductsInStock += quantity;
            }
            else
            {
                company.ProductsInStock -= quantity;
            }
            bool updatedCompany = _productRepos.UpdateCompanyProductStock(company);
            return updatedCompany;
        }

        public void CheckProductStockChange(int productID, int productNewStock)
        {
            Product product = GetProductByID(productID);
            int quantity;
            if (product.Stock == productNewStock) return;
            else if (product.Stock > productNewStock)
            {
                quantity = product.Stock - productNewStock;
                UpdateCompanyProductStock(product.CompanyID, quantity, "decrease");
            }
            else
            {
                quantity = productNewStock - product.Stock;
                UpdateCompanyProductStock(product.CompanyID, quantity, "increase");

            }

        }

        public bool CreateImage(Image image)
        {
            bool createdImage = _productRepos.CreateImage(image);
            return createdImage;
        }

        public void HandleProductImages(Product product, IFormFileCollection files)
        {
            string fileName;
            string path;

            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var extension = Path.GetExtension(files[i].FileName);
                    fileName = (i == 0) ? product.Image : Guid.NewGuid().ToString() + extension;
                    path = Path.Combine(_environment.WebRootPath, "Img") + "/" + fileName;

                    Image img = new Image();
                    img.Name = fileName;
                    img.ProductID = product.ID;

                    bool createdImage = CreateImage(img);

                    using (FileStream fs = System.IO.File.Create(path))
                    {
                        files[i].CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
        }

        public List<Image> GetProductImages(int productID)
        {
            var images = _productRepos.GetImages().Where(x => x.ProductID == productID).ToList();
            return images;
        }

        public int GetProductSales(int productID)
        {
            List<TransactionItem> transactionItems = _productRepos.GetTransactionItems();
            int productSales = transactionItems.Where(x => x.ProductID == productID).Sum(x => x.Quantity);
            return productSales;
        }

        public IEnumerable<Product> GetFilteredProducts(string filterOption, int optionIdentify,  string searchedProduct, int companyID = 0)
        {
            IEnumerable<Product> filteredProducts = null;
            switch (filterOption)
            {
                case "Company":
                    filteredProducts = GetCompanyProducts(optionIdentify);
                    break;
                case "Category":
                    filteredProducts = CategoryFilter(optionIdentify);
                    break;
                case "Price":
                    filteredProducts = PriceFilter(optionIdentify);
                    break;
                case "Rating":
                    filteredProducts = RatingFilter(optionIdentify);
                    break;
                case "Most Sold":
                    filteredProducts = SalesFilter();
                    break;
                case "Most Revenue":
                    filteredProducts = SalesFilter("Most Revenue");
                    break;
                case "Search":
                    filteredProducts = SearchFilter(searchedProduct);
                    break;
                default:
                    break;
            }
           
            return companyID == 0 ? filteredProducts : filteredProducts?.Where(x => x.CompanyID == companyID);
        }

        public IEnumerable<Product> CategoryFilter(int categoryID)
        {
            var filteredProducts = GetAllProducts().Where(x => x.CategoryID == categoryID);
            return filteredProducts;
        }

        public IEnumerable<Product> PriceFilter(int order)
        {
            IEnumerable<Product> filteredProducts;
            if (order == 1)
            {
                filteredProducts = GetAllProducts().OrderBy(x => x.Price);
                return filteredProducts;
            }
            filteredProducts = GetAllProducts().OrderByDescending(x => x.Price);
            return filteredProducts;
        }

        public IEnumerable<Product> RatingFilter(int order)
        {
            IEnumerable<Product> filteredProducts;
            if (order == 1)
            {
                filteredProducts = GetAllProducts().OrderByDescending(x => x.AverageRating);
                return filteredProducts;
            }
            filteredProducts = GetAllProducts().OrderBy(x => x.AverageRating);
            return filteredProducts;
        }

        public List<Product> SalesFilter(string saleOption = "Most Sold")
        {
            var mostSoldProductsQuery = _productRepos.GetTransactionItems()
                                                        .GroupBy(item => item.ProductID)
                                                        .Select(group => new
                                                        {
                                                            ProductID = group.Key,
                                                            QuantitySold = group.Sum(item => saleOption == "Most Sold" ? item.Quantity : item.Quantity * item.Product.Price)
                                                        })
                                                        .OrderByDescending(result => result.QuantitySold);

            List<Product> mostSoldProducts = new List<Product>();
            foreach(var item in mostSoldProductsQuery)
            {
                var product = GetProductByID(item.ProductID);
                mostSoldProducts.Add(product);
            }
            return mostSoldProducts;
        }

        public List<Product> PageFilter(int pageNumber, IEnumerable<Product> products = null)
        {
            
            List<Product> filteredProducts = new List<Product>();

            int productsPerPage = 12;
            if(pageNumber == 1 | pageNumber == 0)
            {
                filteredProducts = products.Take(productsPerPage).ToList();
                return filteredProducts;
            }
            int startingIndex = (pageNumber - 1) * productsPerPage;
            for (int i = startingIndex; i < startingIndex + productsPerPage; i++)
            {
                if (i == products.Count()) break;

                filteredProducts.Add(products.ElementAt(i));

            }
            return filteredProducts;
        }

        public IEnumerable<Review> FilterReviews(int pageNumber)
        {
            List<Review> filteredReviews = new List<Review>();
            List<Review> allReviews = GetReviews().OrderByDescending(x => x.Date).ToList();
            int reviewsPerPage = 5;
            if (pageNumber == 1 | pageNumber == 0)
            {
                filteredReviews = GetReviews().OrderByDescending(x => x.Date).Take(reviewsPerPage).ToList();
                return filteredReviews;
            }
            for (int i = (pageNumber - 1) * reviewsPerPage; i < allReviews.Count; i++)
            {
                filteredReviews.Add(allReviews[i]);
            }
            return filteredReviews;
        }

        public IEnumerable<Product> SearchFilter(string searchedProduct)
        {
            IEnumerable<Product> products;
            var categorySearched = GetAllCategories().Where(x => x.Name.Contains(searchedProduct, StringComparison.OrdinalIgnoreCase));
            if (categorySearched.Count() != 0)
            {
                var categoryFound = categorySearched.First();
                products = GetAllProducts().Where(x => x.CategoryID == categoryFound.ID);
            }
            else
            {
                products = GetAllProducts().Where(x => x.Name.Contains(searchedProduct, StringComparison.OrdinalIgnoreCase));
            }
           
            return products;

        }
        public IEnumerable<Product> GetPopularProducts()
        {
            var products = _productRepos.GetPopularProducts();
            return products;
        }
    }
}
