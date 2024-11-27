using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models
{
    public class Refund
    {
        [Key]
        public int ID { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
        public virtual AppUser User { get; set; }

        [ForeignKey("Products")]
        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Transactions")]
        public int? TransactionID { get; set; }
        public virtual Transaction Transaction { get; set; }

        [ForeignKey("TransactionItems")]
        public int? TransactionItemID { get; set; }
        public virtual TransactionItem TransactionItem { get; set; }
    }
}
