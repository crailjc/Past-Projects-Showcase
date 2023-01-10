using System.Collections.Generic;
using CoursePlanner.Mangers;

namespace CoursePlanner.Models
{
    public class CommentModel
    {
        public List<Comment> comments = new();

        public CommentModel(int planID)
        {
            comments = CommentManager.GetComments(planID);
        }
    }
}
