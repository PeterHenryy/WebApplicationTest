using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class ProductIndexViewModel
    {
        public List<Product> Products { get; set; }

        public IEnumerable<Product> GetMostPopularProducts(int categoryID = 0)
        {
            if (categoryID == 0)
            {
                var products = Products.OrderByDescending(x => x.AverageRating);
                if (products.Count() > 5)
                {
                    return products.Take(6);
                }
                return products;
            }
            var popularProducts = Products.Where(x => x.CategoryID == categoryID).OrderByDescending(x => x.AverageRating);
            if (popularProducts.Count() > 5)
            {
                return popularProducts.Take(6);
            }
            return popularProducts;
        }
        public AppUser User { get; set; }
    }
}
