using CoursePlanner.Models;
using System.Collections.Generic;
using Dapper;
using CoursePlanner.Managers;

namespace CoursePlanner.Managers
{
    public class PlanManager
    {

        public static bool PersonOwnsPlan(int personID, int planID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Store the statement
            string stmt = @"CALL GetNumOwnedPlans(@PlanID, @personID)";
            // Execute the SQL statement that gets the number of plans
            // Where the person is either the student or advisor
            // and it matched the planID
            int count = connection.ExecuteScalar<int>(stmt,
                new { planID, personID });
            // Return if the count is greater than 0
            return count > 0;
        }

        public static void DeleteSemesterCourse(int semesterID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Store the statement to delete the course-semester relationship
            string stmt =
                @"CALL DeleteSemesterCourse(@semesterID)";
            // Execute the SQL statement to delete the course-semester
            // relationship in the database
            connection.Execute(stmt, new { semesterID = semesterID });
        }

        public static void InsertSemesterCourse(int semesterID, int courseID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Store SQL statement to add the given course to the
            // semester
            string stmt = @"CALL InsertSemesterCourse (@semesterID, @courseID)";
            // Execute SQL statement to add the given course to the
            // given semester
            connection.Execute(stmt, new { semesterID = semesterID, courseID = courseID });
        }

        /**
         * Method to delete a plan from a user's record using planID.
         */
        public static bool DeletePlan(int planID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            string deletePlan = @"CALL DeletePlan(@PlanID)";
            return connection.Execute(deletePlan, new { PlanID = planID }) > 0;
        }

        public static bool PlanNameExist(string miamiID, string planName)
        {
            // Get all of the plans that this miamiID can view
            List<PlanDetail> plans = GetPlanDetails(miamiID);
            // Iterate through each plan
            foreach (PlanDetail p in plans)
            {
                // Check if the plan name is equal to the
                // given plan name
                if (p.PlanName == planName)
                {
                    // Return that it was found
                    return true;
                }
            }
            // Return false since it wasn't found
            return false;

        }

        /// <summary>
        /// Gets all of the plan information for the given
        /// person
        /// </summary>
        /// <param name="person">The person to gather
        /// the plan information for</param>
        /// <returns></returns>
        public static List<Plan> SelectAllPlans(int person)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return the query of the plans that belong to the person
            return connection.Query<Plan>(
                "CALL SelectAllPlans(@personID)",
                new { personID = person }).AsList<Plan>();
        }

        public static List<PlanDetail> GetPlanDetails(string miamiID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Store the SQL statement that will get all of the plans
            // accessible by the miamiID, through the advisor role
            // or personal ownership
            string stmt = @"CALL GetPlanDetails(@miamiID, @personID)";
            // Get the personID
            int personID = PersonManager.GetPersonID(miamiID);
            // Return the list of plans found in the query
            return connection.Query<PlanDetail>(stmt, new { miamiID, personID }).AsList<PlanDetail>();
        }

        /// <summary>
        /// Inserts a new plan with the given information
        /// </summary>
        /// <param name="plan">The plan information to add
        /// to the database</param>
        /// <returns></returns>
        public static int InsertNewPlan(Plan plan)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Create the insert query
            string insertQuery = @"CALL InsertNewPlan(@PersonID, @PlanName, @EnrollYear, @EnrollSeason)";
            // Execute the insert
            return connection.ExecuteScalar<int>(insertQuery, plan);

        }

        public static bool InsertNewDegreePlan(DegreePlan degreePlan)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Create the insert query
            string insertQuery = @"CALL InsertNewDegreePlan(@PlanID, @DegreeID)";
            // Execute the insert and return true if affected rows are greater than 0
            return connection.Execute(insertQuery, degreePlan) > 0;
        }

        public static bool InsertNewSemesterPlan(SemesterPlan semesterPlan)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            string insertQuery = @"CALL InsertNewSemesterPlan(@PlanID, @Season, @CurrentYear)";
            return connection.Execute(insertQuery, semesterPlan) > 0;
        }

        public static bool RemoveSemesters(int planID, int semesterID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Delete statement
            string stmt = @"CALL RemoveSemesters(@planID, @semesterID)";
            // Execute delete and ensure a change was made
            return connection.Execute(stmt, new { planID, semesterID }) > 0;
        }

        /// <summary>
        /// Gets the detailed plan based on the given planID
        /// </summary>
        /// <param name="planID">The ID of the plan you
        /// want to get more information on</param>
        /// <returns>A list of the detailed plan</returns>
        public static List<DetailedPlan> GetDetailedPlans(int planID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Get the detailed plan
            return connection.Query<DetailedPlan>(
                @"CALL GetDetailedPlans(@planID)", new { planID }).AsList();
        }

        public static int GetPlanYear(int planID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Execute SQL query to get the plan year
            return connection.ExecuteScalar<int>(@"CALL GetPlanYear(@planID)",
                                    new { planID });
        }

        public static int GetDegreeID(int planID)
        {
            // Get the database connection
            using var connection = Connection.GetConnection();
            // Execute SQL query to get the degreeID of the given plan
            // Returns the ID found
            return connection.ExecuteScalar<int>(@"CALL GetDegreeID(@planID)",
                                    new { planID = planID });
        }

        /// <summary>
        /// Adds the specificed course to the specified semester
        /// </summary>
        /// <param name="semesterID">The ID of the semester you
        /// want to add to</param>
        /// <param name="courseID">The ID of the course you want
        /// to add to the semester</param>
        /// <returns>True if it was added, false otherwise</returns>
        public static bool AddCourseToSemester(int semesterID, int courseID)
        {
            // Get the connecetion
            using var connection = Connection.GetConnection();
            // Check if the course is already within the semester
            if (SemesterContainsCourse(semesterID, courseID))
            {
                return false;
            }
            // Create the insert query
            string insertQuery = @"CALL AddCourseToSemester(@semesterID, @courseID)";
            // Execute the query
            int numRowsAffected = connection.Execute(insertQuery,
                new {
                    semesterID,
                    courseID
                });
            // Return true if there was affected rows (should be 1)
            return numRowsAffected > 0;
        }

        /// <summary>
        /// Removes the given course from the semester.
        /// </summary>
        /// <param name="semesterID">The ID of the semester
        /// containing the course</param>
        /// <param name="courseID">The ID of the course you
        /// want to remove </param>
        /// <returns>True if it was removed, false otherwise</returns>
        public static bool RemoveCourseFromSemester(int semesterID,
            int courseID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Delete from the table
            int numRowsAffected = connection.Execute(
                @"CALL RemoveCourseFromSemester (@courseID, @semesterID)",
                new
                {
                    courseID,
                    semesterID
                });
            // return true to indicate successful deletion
            return numRowsAffected > 0;
        }

        /// <summary>
        /// Checks if the given course is within the given
        /// semester.
        /// </summary>
        /// <param name="semesterID">The semesterID to check</param>
        /// <param name="courseID">The courseID to check</param>
        /// <returns></returns>
        public static bool SemesterContainsCourse(int semesterID, int courseID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Get the number of relations with the
            // given course numbers
            int num = connection.ExecuteScalar<int>(
                @"CALL SemesterContainsCourse (@semesterID, @courseID)",
                new
                {
                    semesterID,
                    courseID
                });
            // returns true if there is at least 1 relation
            return num > 0;
        }

        /// <summary>
        /// Adds a given semester to a plan
        /// </summary>
        /// <param name="planID">The planID to add the semester to</param>
        /// <param name="season">The season of the new semester</param>
        /// <param name="year">The year of the new semester</param>
        /// <returns>The semesterID if added, -1 otherwise</returns>
        public static int AddSemesterToPlan(
            int planID, string season, int year)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Check if the season already exists
            if (PlanContainsSemester(planID, season, year))
            {
                // Return -1 to indicate no new season was
                // created
                return -1;
            }
            // Create the INSERT query
            string insertQuery = @"INSERT INTO
                SemesterPlan (planID, season, currentYear)
                VALUES (@planID, @season, @year)";
            // Attempt to execute the query
            int numRowsAffected = connection.Execute(insertQuery,
                new
                {
                    planID,
                    season,
                    year
                });
            // Check if the number of rows affects is greater than 1
            if (numRowsAffected > 1)
            {
                // Get the semester ID
                int semesterID = connection.ExecuteScalar<int>(
                    @"SELECT semesterID
                    FROM SemesterPlan sp
                    WHERE sp.planID = @planID and sp.season = @season
                        and sp.currentYear = @year",
                    new
                    {
                        planID,
                        season,
                        year
                    });
                // Return the semesterID
                return semesterID;
            }
            else
            {
                // Return -1 since the semester wasn't created
                return -1;
            }
        }

        /// <summary>
        /// Removes the given semester from the plan
        /// </summary>
        /// <param name="planID">The id of the plan you want
        /// to modify</param>
        /// <param name="semesterID">the ID of the semester
        /// to remove</param>
        /// <returns></returns>
        public static bool RemoveSemesterFromPlan(int planID,
            int semesterID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Delete from the table
            int numRowsAffected = connection.Execute(
                @"CALL RemoveSemesterFromPlan (@semesterID, @planID)",
                new
                {
                    planID,
                    semesterID
                });
            // return true to indicate successful deletion
            return numRowsAffected > 0;
        }

        /// <summary>
        /// Checks if the given season is contained
        /// within the semester
        /// </summary>
        /// <param name="planID">The ID of the plan
        /// you want to check</param>
        /// <param name="season">The season of the semester
        /// you are checking</param>
        /// <param name="year">The year of the semester
        /// you are checking</param>
        /// <returns>True if the plan has the semester,
        /// false otherwise</returns>
        public static bool PlanContainsSemester(int planID, string season, int year)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Get the number of relations with the
            // given course numbers
            int num = connection.ExecuteScalar<int>(
                @"CALL PlanContainsSemester(@planID, @season, @year)",
                new
                {
                    planID,
                    season,
                    year
                });
            // returns true if there is at least 1 relation
            return num > 0;
        }

        public static System.Exception NotYourPlanIDException()
        {
            throw new System.Exception();
        }

    }
}
