using System.Collections.Generic;
using CoursePlanner.Managers;
using CoursePlanner.Models;
using Dapper;

namespace CoursePlanner.Mangers
{
    public class CommentManager
    {
        public static List<Comment> GetComments(int PlanID)
        {
            using var connection = Connection.GetConnection();
            var stmt = @"CALL GetComments(@planID)";
            return
                connection.Query<Comment>(stmt,
                                new { planID = PlanID }).AsList<Comment>();
        }

        public static bool AddComment(Comment comment)
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"CALL AddComment(@planID, @content, @personID)";
            command.Parameters.AddWithValue("@planID", comment.PlanID);
            command.Parameters.AddWithValue("@content", comment.Text);
            command.Parameters.AddWithValue("@personID", comment.PersonID);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }


    }
}
