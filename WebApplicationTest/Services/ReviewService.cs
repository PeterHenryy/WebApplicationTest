using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepos;

        public ReviewService(ReviewRepository reviewRepos)
        {
            _reviewRepos = reviewRepos;
        }

        public bool Create(Review review)
        {
            bool createdReview = _reviewRepos.Create(review);
            return createdReview;
        }

        public bool Delete(int reviewID)
        {
            bool deletedReview = _reviewRepos.Delete(reviewID);
            return deletedReview;
        }

        public bool Update(Review review)
        {
            bool updatedReview = _reviewRepos.Update(review);
            return updatedReview;
        }

        public IEnumerable<Review> GetUserReviews(int userID)
        {
            var userReviews = _reviewRepos.GetSpecificUserReviews(userID);
            return userReviews;
        }

        public Review GetReviewByID(int reviewID)
        {
            Review review = _reviewRepos.GetReviewByID(reviewID);
            return review;
        }

        public Product GetProductByID(int? productID)
        {
            Product product = _reviewRepos.GetProductByID(productID);
            return product;
        }

        public bool UpdateProductRating(Product product)
        {
            bool updatedProduct = _reviewRepos.UpdateProductRating(product);
            return updatedProduct;
        }

        public IEnumerable<Review> GetReviews()
        {
            var reviews = _reviewRepos.GetReviews();
            return reviews;
        }

        public void CalculateProductAverageRating(int? productID)
        {
            var product = GetProductByID(productID);
            var reviews = GetReviews();

            var productReviews = reviews.Where(x => x.ProductID == productID);
            if (productReviews.Count() != 0)
            {
                double rating = productReviews.Sum(x => x.Rating) / (double)productReviews.Count();
                double roundedRating = Math.Round(rating, 1);
                product.AverageRating = roundedRating;
                UpdateProductRating(product);
            }
            else
            {
                product.AverageRating = 0;
                UpdateProductRating(product);
            }

        }
    }
}
