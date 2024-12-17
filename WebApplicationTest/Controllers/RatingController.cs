using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models.ViewModels;
using WebApplicationTest.Services;

namespace WebApplicationTest.Controllers
{
    public class RatingController : Controller
    {
        private readonly RatingService _ratingService;

        public RatingController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDetailsViewModel viewModel)
        {
            if (viewModel.LikeForm != null)
            {
                // Create a like
                bool createdLike = _ratingService.CreateLike(viewModel.LikeForm);
            }
            else if (viewModel.DislikeForm != null)
            {
                // Create a dislike
                bool createdDislike = _ratingService.CreateDislike(viewModel.DislikeForm);
            }
            viewModel.Likes = _ratingService.GetLikes().ToList();
            viewModel.Dislikes = _ratingService.GetDislikes().ToList();
            // After creating like or dislike, fetch the updated counts
            int likeCount = viewModel.GetLikesPerReview(viewModel.LikeForm?.ReviewID ?? viewModel.DislikeForm.ReviewID);
            int dislikeCount = viewModel.GetDislikesPerReview(viewModel.LikeForm?.ReviewID ?? viewModel.DislikeForm.ReviewID);

            // Return JSON response with updated counts
            return Json(new { likeCount = likeCount, dislikeCount = dislikeCount });
        }
        [HttpPost]
        public IActionResult Delete(int ratingChoice, int ratingID, int productID)
        {
            bool success;

            if (ratingChoice == 1)
            {
                success = _ratingService.DeleteLike(ratingID);
            }
            else
            {
                success = _ratingService.DeleteDislike(ratingID);
            }

            if (success)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete rating." });
            }
        }

        [HttpGet]
        public IActionResult GetReviewInformation(int reviewID)
        {
            int likes = _ratingService.GetLikes()?.Where(x => x.ReviewID == reviewID).Count() ?? 0;
            int dislikes = _ratingService.GetDislikes()?.Where(x => x.ReviewID == reviewID).Count() ?? 0;

            return Json(new
            {
                success = true,
                likeCount = likes,
                dislikeCount = dislikes
            });
        }
    }
}
