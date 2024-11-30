using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class ProductsCardViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public AppUser User { get; set; }
        public bool CompanyProducts { get; set; }
    }
}
