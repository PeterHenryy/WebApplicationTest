using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Dislike
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
        [ForeignKey("Reviews")]
        public int? ReviewID { get; set; }
        [ForeignKey("Comments")]
        public int? CommentID { get; set; }

    }
}
