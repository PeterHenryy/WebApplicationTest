using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Transaction
    {
        [Key]
        public int ID { get; set; }
        public double Total { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public int ItemsBought { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PaymentType { get; set; }
        public string? CouponCode { get; set; }
        public double CouponPercentage { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
    }
}
