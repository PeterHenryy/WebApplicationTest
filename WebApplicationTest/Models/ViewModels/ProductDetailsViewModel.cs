using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
        public bool HasUserBoughtProduct { get; set; }
        public Comment Comment { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<Dislike> Dislikes { get; set; }
        public Like LikeForm { get; set; }
        public Dislike DislikeForm { get; set; }
        public AppUser CurrentUser { get; set; }
        public int ProductID { get; set; }
        public bool HasUserReviewedProduct { get; set; }
        public List<Image> ProductImages { get; set; }
        public int ProductSales { get; set; }
        public Review Review { get; set; }
        public IEnumerable<Comment> GetCommentsForReview(int reviewID)
        {
            var comments = Comments.Where(x => x.ReviewID == reviewID);
            return comments;
        }

        public int GetLikesPerReview(int? reviewID)
        {
            int likeCount = Likes?.Where(x => x.ReviewID == reviewID).Count() ?? 0;
            return likeCount;
        }

        public int GetDislikesPerReview(int? reviewID)
        {
            int dislikeCount = Dislikes?.Where(x => x.ReviewID == reviewID).Count() ?? 0;
            return dislikeCount;
        }

        public int GetLikePerComment(int commentID)
        {
            int likeCount = Likes.Where(x => x.CommentID == commentID).Count();
            return likeCount;
        }

        public int GetDislikesPerComment(int commentID)
        {
            int dislikeCount = Dislikes.Where(x => x.CommentID == commentID).Count();
            return dislikeCount;
        }

        public Like FindUserLikeInReview(int userID, int reviewID)
        {
            var userLikeInReview = Likes.Where(x => x.UserID == userID).SingleOrDefault(x => x.ReviewID == reviewID);
            return userLikeInReview;
        }

        public Dislike FindUserDislikeInReview(int userID, int reviewID)
        {
            var userDislikeInReview = Dislikes.Where(x => x.UserID == userID).SingleOrDefault(x => x.ReviewID == reviewID);
            return userDislikeInReview;
        }

        public Like FindUserLikeInComment(int userID, int commentID)
        {
            var userLikeInComment = Likes.Where(x => x.UserID == userID).SingleOrDefault(x => x.CommentID == commentID);
            return userLikeInComment;
        }

        public Dislike FindUserDislikeInComment(int userID, int commentID)
        {
            var userDislikeInComment = Dislikes.Where(x => x.UserID == userID).SingleOrDefault(x => x.CommentID == commentID);
            return userDislikeInComment;
        }

        public int CalculateRatingPercentagePerStar(int starQuantity)
        {
            int starQuantityOnReview = GetStarQuantityPerReview(starQuantity);
            int reviewCount = Reviews.Count();
            int percentage = Convert.ToInt32((starQuantityOnReview / (double)reviewCount) * 100);
            return percentage;
        }

        public int GetStarQuantityPerReview(int starQuantity)
        {
            int starQuantityOnReview = Reviews.Count(x => x.Rating == starQuantity);
            return starQuantityOnReview;
        }

    }
}
