using WebApplicationTest.Models;
using WebApplicationTest.Models.Repositories;

namespace WebApplicationTest.Services
{
    public class ContactService
    {
        private readonly ContactRepository _contactRepository;

        public ContactService(ContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public bool Create(Contact contactRequest)
        {
            bool createdRequest = _contactRepository.Create(contactRequest);
            return createdRequest;
        }
    }
}
