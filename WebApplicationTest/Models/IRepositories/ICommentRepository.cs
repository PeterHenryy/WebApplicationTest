namespace WebApplicationTest.Models.IRepositories
{
    public interface ICommentRepository
    {
        bool Create(Comment comment);
        bool Update(Comment comment);
        bool Delete(int commentID);
        Comment GetCommentByID(int commentID);
    }
}
