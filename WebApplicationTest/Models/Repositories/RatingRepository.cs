using WebApplicationTest.Data;
using WebApplicationTest.Models.IRepositories;

namespace WebApplicationTest.Models.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateLike(Like like)
        {
            try
            {
                _context.Likes.Add(like);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool CreateDislike(Dislike dislike)
        {
            try
            {
                _context.Dislikes.Add(dislike);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public bool DeleteLike(int likeID)
        {
            try
            {
                var like = _context.Likes.SingleOrDefault(x => x.ID == likeID);
                _context.Likes.Remove(like);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool DeleteDislike(int dislikeID)
        {
            try
            {
                var dislike = _context.Dislikes.SingleOrDefault(x => x.ID == dislikeID);
                _context.Dislikes.Remove(dislike);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public IEnumerable<Like> GetLikes()
        {
            var likes = _context.Likes.ToList();
            return likes;
        }

        public IEnumerable<Dislike> GetDislikes()
        {
            var dislikes = _context.Dislikes.ToList();
            return dislikes;
        }
    }
}
