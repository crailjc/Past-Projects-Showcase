using CoursePlanner.Models;
using Dapper;

namespace CoursePlanner.Managers
{
    public class PersonManager
    {
        // Has an error when person does not exist!
        public static Person GetPerson(string miamiID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Select the first person in the database with the same
            // miamiID
            // NOTE: Will throw an error if the person doesn't exists. It's
            // recommended that you use ContainsPerson() first to ensure
            // that the person exists.
            return connection.QueryFirst<Person>(
                "CALL GetPerson(@mID)",
                new { mID = miamiID });
        }

        public static int GetPersonID(string miamiID)
        {
            // Check if the person exists
            if (ContainsPerson(miamiID))
            {
                // Get the person
                Person person = GetPerson(miamiID);
                // Return the ID of the retrieved person
                return person.PersonID;
            }
            else
            {
                // Create a new person, and return the new ID
                // of the individual
                return InsertPerson(miamiID);
            }
        }

        public static int InsertPerson(string miamiID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Execute the SQL query to add the new person,
            // and get the new ID that represents them in the database
            return connection.ExecuteScalar<int>(
                "CALL InsertPerson(@miamiID)",
                new { miamiID});
        }

        public static bool ContainsPerson(string miamiID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Execute the SQL query that gets the number of people
            // with the given miamiID, and will return true if the number
            // is greater than 0, false otherwise
            return connection.ExecuteScalar<int>(
                "CALL ContainsPerson(@mID)",
                new { mID = miamiID }) > 0;
        }
    }
}
