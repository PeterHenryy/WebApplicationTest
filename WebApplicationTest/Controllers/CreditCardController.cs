using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;
using WebApplicationTest.Models.ViewModels;

namespace WebApplicationTest.Controllers
{
    public class CreditCardController : Controller
    {
        private readonly CreditCardService _cardService;
        private readonly UserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppUser _currentUser;

        public CreditCardController(CreditCardService cardService, UserService userService, UserManager<AppUser> userManager)
        {
            _cardService = cardService;
            _userService = userService;
            _userManager = userManager;
            _currentUser = _userService.GetCurrentUser();
        }

        public IActionResult UserCards(int userID)
        {
            var userCards = _cardService.GetSpecificUserCards(userID);
            return View(userCards);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var creditCardVM = new CreditCardViewModel();
            return View(creditCardVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreditCardViewModel cardVM)
        {
            cardVM.CreditCard.UserID = _currentUser.Id;
            bool createdCard = _cardService.Create(cardVM.CreditCard);
            if (createdCard)
            {
                _currentUser.HasCreditCard = true;
                await _userManager.UpdateAsync(_currentUser);
                return RedirectToAction("UserCards", "CreditCard", new { userID = _currentUser.Id });
            }
            return View(cardVM.CreditCard);
        }

        [HttpGet]
        public IActionResult Update(int cardID)
        {
            var creditCard = _cardService.GetCreditCardByID(cardID);
            return View(creditCard);
        }

        [HttpPost]
        public IActionResult Update(CreditCard creditCard)
        {
            bool updatedCard = _cardService.Update(creditCard);
            if (updatedCard)
            {
                return RedirectToAction("UserCards", "CreditCard", new { userID = _currentUser.Id });
            }
            return View(creditCard);
        }

        public async Task<IActionResult> Delete(int cardID)
        {
            _cardService.Delete(cardID);
            var userCards = _cardService.GetSpecificUserCards(_currentUser.Id);
            if (!userCards.Any())
            {
                _currentUser.HasCreditCard = false;
                await _userManager.UpdateAsync(_currentUser);
            }
            return RedirectToAction("UserCards", "CreditCard", new { userID = _currentUser.Id });
        }

    }
}
