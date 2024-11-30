using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models;
using WebApplicationTest.Services;

namespace WebApplicationTest.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contactRequest)
        {
            bool createdRequest = _contactService.Create(contactRequest);
            return View();
        }
    }
}
