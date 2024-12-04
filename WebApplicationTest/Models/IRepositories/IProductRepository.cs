namespace WebApplicationTest.Models.IRepositories
{
    public interface IProductRepository
    {
        bool Create(Product product);
        bool Update(Product product);
        bool Delete(int productID);
        Product GetProductByID(int? productID);
        IEnumerable<Product> GetAllProducts();
    }
}
