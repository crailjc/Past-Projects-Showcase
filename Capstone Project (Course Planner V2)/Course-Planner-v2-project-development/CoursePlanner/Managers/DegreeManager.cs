using CoursePlanner.Models;
using System.Collections.Generic;
using Dapper;

namespace CoursePlanner.Managers
{
    public class DegreeManager
    {
        /// <summary>
        /// Gets all of the supported degrees from the database
        /// </summary>
        /// <returns>
        /// List containing information of the available degrees
        /// </returns>
        public static List<Degree> GetAllDegrees()
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Execute SQL query to get all of the available information
            // for the degrees in the database
            return connection.Query<Degree>(
                "SELECT * FROM Degree").AsList<Degree>();
        }

        /// <summary>
        /// Gets all of the course group information tied to the
        /// given degree
        /// </summary>
        /// <param name="degree">
        /// The degree used to identify which course groups are required
        /// to satisfy this degree
        /// </param>
        /// <returns>
        /// List containing information of the course groups tied to the
        /// given degree
        /// </returns>
        public static List<DegreeRequirements> GetDegreeRequirements(Degree degree)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Execute SQL query to get all of the available information
            // on the different required course groups
            // for the given degree 
            return connection.Query<DegreeRequirements>(@"SELECT * FROM
                RequirementCourseGroup rcg WHERE rcg.degreeID = @degreeID",
                new { degreeID = degree.DegreeID }).AsList<DegreeRequirements>();
        }
    }
}
