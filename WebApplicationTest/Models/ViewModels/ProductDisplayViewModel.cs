using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class ProductDisplayViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Company> Companies { get; set; }
        public AppUser User { get; set; }

    }
}
