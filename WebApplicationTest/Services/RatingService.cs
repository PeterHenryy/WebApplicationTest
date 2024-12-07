using WebApplicationTest.Models.Repositories;
using WebApplicationTest.Models;

namespace WebApplicationTest.Services
{
    public class RatingService
    {
        private readonly RatingRepository _ratingRepos;

        public RatingService(RatingRepository ratingRepos)
        {
            _ratingRepos = ratingRepos;
        }

        public bool CreateLike(Like like)
        {
            bool createdLike = _ratingRepos.CreateLike(like);
            return createdLike;
        }

        public bool CreateDislike(Dislike dislike)
        {
            bool createdDislike = _ratingRepos.CreateDislike(dislike);
            return createdDislike;
        }

        public bool DeleteLike(int likeID)
        {
            bool deletedLike = _ratingRepos.DeleteLike(likeID);
            return deletedLike;
        }

        public bool DeleteDislike(int dislikeID)
        {
            bool deletedDislike = _ratingRepos.DeleteDislike(dislikeID);
            return deletedDislike;
        }

        public IEnumerable<Like> GetLikes()
        {
            var likes = _ratingRepos.GetLikes();
            return likes;
        }

        public IEnumerable<Dislike> GetDislikes()
        {
            var dislikes = _ratingRepos.GetDislikes();
            return dislikes;
        }
    }
}
