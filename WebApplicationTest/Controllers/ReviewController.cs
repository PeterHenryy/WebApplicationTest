using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.Identity;
using WebApplicationTest.Models;
using WebApplicationTest.Services;

namespace WebApplicationTest.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;

        public ReviewController(ReviewService reviewService, UserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }

        public IActionResult UserReviews()
        {
            AppUser user = _userService.GetCurrentUser();
            var userReviews = _reviewService.GetUserReviews(user.Id);
            return View(userReviews);
        }

        [HttpPost]
        public IActionResult Create(Review review)
        {
            AppUser user = _userService.GetCurrentUser();
            review.UserID = user.Id;
            bool createdReview = _reviewService.Create(review);
            if (createdReview)
            {
                _reviewService.CalculateProductAverageRating(review.ProductID);
                return RedirectToAction("UserReviews", "Review");
            }
            return View(review);
        }

        [HttpGet]
        public IActionResult Update(int reviewID)
        {
            Review review = _reviewService.GetReviewByID(reviewID);
            return View(review);
        }

        [HttpPost]
        public IActionResult Update(Review review)
        {
            bool updatedReview = _reviewService.Update(review);
            if (updatedReview)
            {
                _reviewService.CalculateProductAverageRating(review.ProductID);
                return RedirectToAction("UserReviews", "Review");
            }
            return View(review);
        }

        public IActionResult Delete(int reviewID)
        {
            Review review = _reviewService.GetReviewByID(reviewID);
            bool deletedReview = _reviewService.Delete(reviewID);
            if (deletedReview)
            {
                _reviewService.CalculateProductAverageRating(review.ProductID);
            }
            return RedirectToAction("UserReviews", "Review");
        }

    }
}
