using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool Delete(int reviewID)
        {
            try
            {
                Review review = GetReviewByID(reviewID);
                _context.Reviews.Remove(review);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public Review GetReviewByID(int reviewID)
        {
            Review review = _context.Reviews.SingleOrDefault(x => x.ID == reviewID);
            return review;
        }

        public IEnumerable<Review> GetSpecificUserReviews(int userID)
        {
            var userReviews = _context.Reviews.Include(x => x.Product).Where(x => x.UserID == userID);
            return userReviews;
        }

        public bool Update(Review review)
        {
            try
            {
                _context.Reviews.Update(review);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public Product GetProductByID(int? productID)
        {
            Product product = _context.Products.SingleOrDefault(x => x.ID == productID);
            return product;
        }

        public bool UpdateProductRating(Product product)
        {
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public IEnumerable<Review> GetReviews()
        {
            var reviews = _context.Reviews.ToList();
            return reviews;
        }
    }
    
}
