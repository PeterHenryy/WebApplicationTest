using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class DeliveryOption
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
    }
}
