using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class CreditCard
    {
        [Key]
        public int ID { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int CVV { get; set; }
        public DateOnly Expiry { get; set; }

        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
    }
}
