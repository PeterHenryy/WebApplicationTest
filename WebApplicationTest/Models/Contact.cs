using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest.Models
{
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
