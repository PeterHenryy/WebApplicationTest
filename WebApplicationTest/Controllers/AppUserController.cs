using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using WebApplicationTest.Helpers.Enums;
using WebApplicationTest.Models;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Services;

namespace WebApplicationTest.Controllers
{
    public class AppUserController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly UserService _userService;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IBlobService _blobService;
        private readonly AppUser _currentUser;

        public AppUserController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManger, UserService userService ,ShoppingCartService shoppingCartService ,  IBlobService blobService )
        {
            _userManager = userManger;
            _userService = userService;
            _shoppingCartService = shoppingCartService;
            _blobService = blobService;
            _signInManager = signInManager;
            _currentUser = userService.GetCurrentUser();

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<RedirectToActionResult> Register(AppUser appUser)
        {
            await HandleBlob(appUser);
            var role = UserRolesEnum.Customer.ToString();
            appUser.ProfilePicture = "user-solid.svg";
            var userRegister = await _userManager.CreateAsync(appUser);
            var assignRole = await _userManager.AddToRoleAsync(appUser, role);

            return RedirectToAction("Login", "AppUser");
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null, int cartItemQuantity = 0, int cartItemProductID = 0)
        {

            ViewData["returnUrl"] = returnUrl;
            ViewData["cartItemQuantity"] = cartItemQuantity;
            ViewData["cartItemProductID"] = cartItemProductID;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AppLogin appLogin, string returnUrl = "", int cartItemQuantity = 0, int cartItemProductID = 0)
        {
            if (appLogin.Username != null)
            {
                AppUser user = await _userManager.FindByNameAsync(appLogin.Username);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username not found.");
                    return View(appLogin);
                }

                try
                {
                    bool correctPassword = user.Password.Equals(appLogin.Password);
                    if (correctPassword)
                    {
                        await _signInManager.SignInAsync(user, false);
                        if (cartItemProductID != 0 && cartItemQuantity != 0)
                        {
                            bool addedItemToCart = _shoppingCartService.AddItemToCart(cartItemProductID, cartItemQuantity, user.Id);
                        }
                        if (!String.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect("https://the-flaming-shop-c7c312b11357.herokuapp.com/" + returnUrl);
                        }
                        return RedirectToAction("Index", "Product");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid password.");
                    return View(appLogin);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                    return View(appLogin);
                }
            }

            return View(appLogin);

        }

        public async Task<RedirectToActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "AppUser");
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View(_currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppUser updatedUser)
        {
            await HandleBlob(updatedUser);
            updatedUser.SecurityStamp = Guid.NewGuid().ToString();
            AppUser mappedUser = await _userService.MapUserUpdates(updatedUser, _currentUser, _userManager);
            var user = await _userManager.UpdateAsync(mappedUser);
            return RedirectToAction("Index", "Product");
        }

        public async Task HandleBlob(AppUser updatedUser)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                bool uploadedBlob = await _blobService.UploadBlob(files[0].FileName, files[0], new Blob());
                updatedUser.ProfilePicture = _blobService.GetBlob(files[0].FileName);
                _userService.HandleUserProfilePicture(files);
            }
            else
            {
                updatedUser.ProfilePicture = "user-solid.svg";
            }
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(AppUser admin)
        {
            admin.CompanyID = _currentUser.CompanyID;
            admin.IsAdmin = true;
            var registeredAdmin = await _userManager.CreateAsync(admin);
            var role = UserRolesEnum.Admin.ToString();
            var assignedRole = await _userManager.AddToRoleAsync(admin, role);
            return RedirectToAction("CompanyStats", "Company");
        }

        public IActionResult About()
        {
            return View();
        }

        
    }
}
