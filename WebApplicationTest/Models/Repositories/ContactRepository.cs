using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Contact contactRequest)
        {
            try
            {
                _context.ContactRequests.Add(contactRequest);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
