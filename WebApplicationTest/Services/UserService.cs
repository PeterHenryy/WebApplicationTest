using Microsoft.AspNetCore.Identity;
using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public UserService(IHttpContextAccessor httpContext, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _httpContext = httpContext;
            _userManager = userManager;
            _environment = environment;
        }

        public AppUser GetCurrentUser()
        {
            var userID = _userManager.GetUserId(_httpContext.HttpContext.User);
            var user = _userManager.FindByIdAsync(userID).Result;
            return user;
        }


        public async Task<AppUser> MapUserUpdates(AppUser updatedUser, AppUser currentUser, UserManager<AppUser> userManager)
        {
            if (!String.IsNullOrEmpty(updatedUser.Email))
            {
                var findUserByEmail = await _userManager.FindByEmailAsync(updatedUser.Email);
                if (findUserByEmail == null)
                {
                    currentUser.Email = updatedUser.Email;
                }
            }
            if (!String.IsNullOrEmpty(updatedUser.FirstName))
            {
                currentUser.FirstName = updatedUser.FirstName;
            }
            if (!String.IsNullOrEmpty(updatedUser.LastName))
            {
                currentUser.LastName = updatedUser.LastName;
            }
            if (!String.IsNullOrEmpty(updatedUser.Password))
            {
                currentUser.Password = updatedUser.Password;
            }
            if (!String.IsNullOrEmpty(updatedUser.ProfilePicture))
            {
                currentUser.ProfilePicture = updatedUser.ProfilePicture;
            }
            if (!String.IsNullOrEmpty(updatedUser.PhoneNumber))
            {
                currentUser.PhoneNumber = updatedUser.PhoneNumber;
            }
            if (!String.IsNullOrEmpty(updatedUser.UserName))
            {
                currentUser.UserName = updatedUser.UserName;
            }
            return currentUser;
        }

        public void HandleUserProfilePicture(IFormFileCollection files)
        {
            var path = Path.Combine(_environment.WebRootPath, "profilepics") + "/" + files[0].FileName;
            using (FileStream fs = System.IO.File.Create(path))
            {
                files[0].CopyTo(fs);
                fs.Flush();
            }
        }
    }
}
