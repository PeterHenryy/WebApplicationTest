using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("AspNetUsers")]
        public int UserID { get; set; }
    }
}
