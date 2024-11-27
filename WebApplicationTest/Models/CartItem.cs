using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }
        public int Quantity { get; set; }
        public double ShippingCost { get; set; }
        public string ShippingOption { get; set; }

        [ForeignKey("ShoppingCarts")]
        public int CartID { get; set; }

        [ForeignKey("Products")]
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
