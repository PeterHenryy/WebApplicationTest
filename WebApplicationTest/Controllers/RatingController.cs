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
                bool createdLike = _ratingService.CreateLike(viewModel.LikeForm);
            }
            else
            {
                bool createdDislike = _ratingService.CreateDislike(viewModel.DislikeForm);
            }
            return RedirectToAction("Details", "Product", new { productID = viewModel.ProductID });
        }

        public IActionResult Delete(int ratingChoice, int ratingID, int productID)
        {
            if (ratingChoice == 1)
            {
                bool deletedLike = _ratingService.DeleteLike(ratingID);
            }
            else
            {
                bool deletedDislike = _ratingService.DeleteDislike(ratingID);
            }
            return RedirectToAction("Details", "Product", new { productID = productID });

        }
    }
}
