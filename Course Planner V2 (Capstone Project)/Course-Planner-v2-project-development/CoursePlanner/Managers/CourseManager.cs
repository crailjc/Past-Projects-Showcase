using CoursePlanner.Models;
using System.Collections.Generic;
using Dapper;
using System.Diagnostics;

namespace CoursePlanner.Managers
{
    public class CourseManager
    {
        /// <summary>
        /// Gets a list of the semesterIDs connected to the given
        /// planID
        /// </summary>
        /// <param name="planID">
        /// The ID of the course you
        /// are adding an equivalence to
        /// </param>
        /// <param name="equivalentCourseID">
        /// The ID of the course that is
        /// equivalent to the other course
        /// </param>
        /// <returns>
        /// True if it was removed, false otherwise
        /// </returns>
        public static List<int> GetSemesterID(int planID)
        {
            using var connection = Connection.GetConnection();
            string stmt = @"CALL GetSemesterID(@planID)";

            return connection.Query<int>(stmt, new { planID = planID}).AsList<int>();
        }

        /// <summary>
        /// Gets all of the courses from the
        /// database
        /// </summary>
        /// <returns>A list of all of courses</returns>
        public static List<Course> GetAllCourses()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return all of the courses
            return
                connection.Query<Course>("CALL GetAllCourses")
                    .AsList<Course>();
        }

        public static List<CourseDetail> GetAllCourseDetails()
        {


            using var connection = Connection.GetConnection();
            string stmt = "CALL GetAllCourseDetails";
            return connection.Query<CourseDetail>(stmt).AsList<CourseDetail>();
        }

        public static List<CourseDetail> GetCoursesForGroup(int groupID)
        {
            // Get the connection to the database
            using var connection = Connection.GetConnection();
            // Prepare the statement
            string stmt = @"CALL GetCoursesForGroup(@groupID)";
            // Execute the query and return the list results
            return connection.Query<CourseDetail>(stmt,
                new { groupID = groupID }).AsList<CourseDetail>();
        }

        public static List<CourseDetail> GetDetailedCousesForGroup(int groupID)
        {
            // Get all of the courses for the specific group
            List<CourseDetail> courses = GetCoursesForGroup(groupID);
            // Get all of th course prereqs
            foreach (CourseDetail cd in courses)
            {
                cd.Prereq = GetAllPreReq(cd.CourseID);
            }
            // Put the responses in a course response
            return courses;
        }

        public static List<string> GetAllPreReq(int courseID)
        {
            using var connection = Connection.GetConnection();
            string stmt = "CALL GetAllPreReq(@courseID)";

            return connection.Query<string>(stmt, new { courseID = courseID }).AsList<string>();
        }

        public static List<CourseGroup> GetCourseGroups(int degreeID, int reqYear)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            string stmt = @"CALL GetCourseGroups(@degreeID, @reqYear)";
            return connection.Query<CourseGroup>(stmt, new { degreeID = degreeID, reqYear = reqYear}).AsList<CourseGroup>();
        }

        // returns the cse core courses using SQL calls
        public static List<DetailedCourse> GetCSECore()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 1")
                    .AsList<DetailedCourse>();
        }

        // returns the cse elective courses using SQL calls
        public static List<DetailedCourse> GetCSEElec()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 3;")
                    .AsList<DetailedCourse>();
        }

        // returns the foundation III - global perspectives courses using SQL calls
        public static List<DetailedCourse> GetFound3()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 5;")
                    .AsList<DetailedCourse>();
        }

        // returns the foundation IV - natural science courses using SQL calls
        public static List<DetailedCourse> GetFound4()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 9;")
                    .AsList<DetailedCourse>();
        }

        // returns the foundation V - math/formal reasoning courses using SQL calls
        public static List<DetailedCourse> GetFound5()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 6;")
                    .AsList<DetailedCourse>();
        }

        // returns the foundation I - english comp courses using SQL calls
        public static List<DetailedCourse> GetFound1()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 8;")
                    .AsList<DetailedCourse>();
        }

        // returns the foundation II - creative arts humanities courses using SQL calls
        public static List<DetailedCourse> GetFound2()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 10;")
                    .AsList<DetailedCourse>();
        }

        // returns the math/stat elective courses using SQL calls
        public static List<DetailedCourse> GetMathStatE()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 7;")
                    .AsList<DetailedCourse>();
        }

        public static List<DetailedCourse> GetCompSci()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 12;")
                    .AsList<DetailedCourse>();
        }

        public static List<DetailedCourse> GetExpL()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 11;")
                    .AsList<DetailedCourse>();
        }

        public static List<DetailedCourse> GetInterC()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 15;")
                    .AsList<DetailedCourse>();
        }

        public static List<DetailedCourse> GetAdvW()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Return one of the courses
            return
                connection.Query<DetailedCourse>("SELECT d.departmentAbbr, c.courseName, c.creditHours as CreditHours, c.courseID FROM Course AS c, Department AS d, CourseGroupCourse AS g, CourseGroup AS cg WHERE c.departmentID = d.departmentID and c.courseID = g.courseID and cg.groupID = g.groupID and g.groupID = 13;")
                    .AsList<DetailedCourse>();
        }




        /// <summary>
        /// Adds the given equivalent course
        /// </summary>
        /// <param name="courseId">The ID of the course you
        /// are adding an equivalence to</param>
        /// <param name="equivalentCourseID">The ID of the
        /// course that is equivalent to the other course
        /// </param>
        /// <returns></returns>
        public static bool AddEquivalentCourse(int courseId,
            int equivalentCourseID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Check if the value already exists
            if (ContainsEquivalentCourse(courseId, equivalentCourseID))
            {
                // Return false
                return false;
            }
            // Insert into the equivalent courses
            connection.Execute(
                @"CALL AddEquivalentCourse(@courseID, @equivCourse)", new { courseID = courseId,
                    equivCourse = equivalentCourseID});
            // Return true to indicate successful insertion
            return true;
        }

        /// <summary>
        /// Checks if the given course is already
        /// equivalent with the specificed eqivalence course
        /// </summary>
        /// <param name="courseId">The ID of the course you
        /// are adding an equivalence to</param>
        /// <param name="equivalentCourseID">The ID of the
        /// course that is equivalent to the other course
        /// </param>
        /// <returns></returns>
        public static bool ContainsEquivalentCourse(int courseId,
            int equivalentCourseID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Get the number of relations with the
            // given course numbers
            int num = connection.ExecuteScalar<int>(
                @"CALL ContainsEquivalentCourse(@courseID, @equivCourse)",
                new
                {
                    courseID = courseId,
                    equivCourse = equivalentCourseID
                });
            // returns true if there is at least 1 relation
            return num > 0;
        }

        /// <summary>
        /// Removes the given course eqivalence
        /// </summary>
        /// <param name="courseId">
        /// The ID of the course you
        /// are adding an equivalence to
        /// </param>
        /// <param name="equivalentCourseID">
        /// The ID of the course that is
        /// equivalent to the other course
        /// </param>
        /// <returns>
        /// True if it was removed, false otherwise
        /// </returns>
        public static bool RemoveEquivalentCourse(int courseId,
            int equivalentCourseID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Delete from the table
            int numRowsAffected = connection.Execute(
                @"CALL RemoveEquivalentCourse (@courseID, @equivCourse)",
                new
                {
                    courseID = courseId,
                    equivCourse = equivalentCourseID
                });
            // return true to indicate successful deletion
            return numRowsAffected > 0;
        }

        /// <summary>
        /// Gets all of the equivalent courses of the given
        /// courseID
        /// from the 
        /// </summary>
        /// <param name="courseID">
        /// The ID of the course you want to
        /// get all of the equivalences from
        /// </param>
        /// <returns>A list of equivalent courses</returns>
        public static List<Course> GetEquivalentCourses(int courseID)
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            // Get the equivalent IDs
            List<int> equivIDs = connection.Query<int>(@"
                SELECT equivCourseID FROM EquivalentCourse
                WHERE courseID = @courseID", new { courseID })
                .AsList();
            Debug.WriteLine(equivIDs);
            // Check if there are any ids
            if (equivIDs.Count > 0)
            {
                // Create the WHERE statement for getting the
                // equivalent courses
                string where = "(";
                for(int i = 0; i < equivIDs.Count; i++)
                {
                    int val = equivIDs[i];
                    where += val + (i + 1 != equivIDs.Count ? ", " : " ");
                }
                where += ")";
                // Return the equivalent courses
                return connection.Query<Course>(@"SELECT * FROM Course c
                    WHERE c.courseID IN " + where).AsList<Course>();
            }
            // Return an empty list
            return new List<Course>();
        }
    }

}
