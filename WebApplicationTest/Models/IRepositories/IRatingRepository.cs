namespace WebApplicationTest.Models.IRepositories
{
    public interface IRatingRepository
    {
        bool CreateLike(Like like);
        bool CreateDislike(Dislike dislike);
        bool DeleteLike(int likeID);
        bool DeleteDislike(int dislikeID);
    }
}
