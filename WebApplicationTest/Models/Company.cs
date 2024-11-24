using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Company
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProductsInStock { get; set; }
        public string Logo { get; set; }
        public int TotalSales { get; set; }
        public double Revenue { get; set; }
    }
}
