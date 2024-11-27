using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Coupon
    {
        [Key]
        public int ID { get; set; }
        public double DiscountPercentage { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Companies")]
        public int? CompanyID { get; set; }

        [ForeignKey("Products")]
        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Categories")]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

    }
}
