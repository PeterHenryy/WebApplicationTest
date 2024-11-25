using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public string Body { get; set; }

        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
        public virtual AppUser User { get; set; }

        [ForeignKey("Reviews")]
        public int? ReviewID { get; set; }

        [ForeignKey("Products")]
        public int? ProductID { get; set; }
    }
}
