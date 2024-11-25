using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Image
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("Products")]
        public int ProductID { get; set; }
    }
}
