using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class ProductSearchViewModel
    {
        public IEnumerable<Product> PopularProducts { get; set; }
        public AppUser User { get; set; }
    }
}
