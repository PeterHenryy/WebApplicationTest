using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models
{
    public class Review
    {
        [Key]
        public int ID { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey("Products")]
        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
        public virtual AppUser User { get; set; }
    }
}
