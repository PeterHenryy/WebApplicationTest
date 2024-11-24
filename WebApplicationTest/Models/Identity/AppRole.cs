using Microsoft.AspNetCore.Identity;

namespace WebApplicationTest.Models.Identity
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
