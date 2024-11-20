using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
