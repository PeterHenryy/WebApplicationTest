namespace WebApplicationTest.Models.IRepositories
{
    public interface IReviewRepository
    {
        bool Create(Review review);
        bool Update(Review review);
        bool Delete(int reviewID);
        IEnumerable<Review> GetSpecificUserReviews(int userID);
        Review GetReviewByID(int reviewID);
        IEnumerable<Review> GetReviews();
    }
}
