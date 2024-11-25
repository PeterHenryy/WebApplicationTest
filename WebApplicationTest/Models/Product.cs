using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationTest.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string ReleaseDate { get; set; }
        public double AverageRating { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }

        [ForeignKey("Companies")]
        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }

        [ForeignKey("Categories")]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
