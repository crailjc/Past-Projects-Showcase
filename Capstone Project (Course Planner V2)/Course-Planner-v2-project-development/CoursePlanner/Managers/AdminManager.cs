using CoursePlanner.Models;
using Dapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CoursePlanner.Managers
{
    public class AdminManager
    {

        public static bool AddCourse(string url)
        {
            //Example URL: ?dep=1&courseNumber=101&courseDescription=This+is+a+test&creditHours=3&group=1&preReq=171@preReq=18
            if (url.Length == 0)
            {
                return false;
            }
            var values = HttpUtility.ParseQueryString(url);
            string[] required = new string[] { "dep", "courseNumber", "courseDescription", "creditHours", "group"};
            // ensure required strings have data
            foreach (string req in required)
            {
                if (values[req] == null || values[req] == "")
                {
                    return false;
                }
            }

            // Fill in all values
            string departmentID = values["dep"];
            string courseDescription = values["courseDescription"];
            string creditHours = values["creditHours"];
            string courseName = values["courseNumber"];
            string groupID = values["group"];

            // Send course insert query out to the db
            string courseID = InsertCourse(departmentID, courseDescription, creditHours, courseName);

            // Put the course group into the db
            InsertCourseWithGroup(courseID, groupID);

            // if there are no prereqs just continue
            if (values["preReq"] == null)
            {
                return true;
            }
            string[] preReqs = values["preReq"].Split(',');
            //Insert list of preReqs into db
            InsertPreReqs(courseID, preReqs);
            return true;
        }

        public static bool UpdateCourse(string url)
        {
            //Example update url: ?courseID=1&dep=1&courseNumber=101&courseDesc=This+is+a+test&creditHours=3&preReq=1&preReq=2&act=update
            if (url.Length == 0)
            {
                return false;
            }
            var values = HttpUtility.ParseQueryString(url);
            // These are the required entries for update
            string[] required = new string[] { "courseID", "dep", "courseNumber", "courseDescription", "creditHours"};
            // ensure required strings have data
            foreach (string req in required)
            {
                if (values[req] == null || values[req] == "")
                {
                    return false;
                }
            }

            string courseID = values["courseID"];
            string departmentID = values["dep"];
            string courseName = values["courseNumber"];
            string courseDescription = values["courseDescription"];
            string creditHours = values["creditHours"];
            //update command goes here
            //update the course with the new info
            UpdateCourseInfo(courseID, departmentID, courseName, courseDescription, creditHours);

            //Remove Prereqs and they will be added later if there are prerequisites
            RemoveAllPreReqs(courseID);

            //changing groups is not a part of this (yet?)
            //remove and re-add all preReqs
            if (values["preReq"] == null)
            {
                return true;
            }
            string[] preReqs = values["preReq"].Split(',');
            
            //insert prereqs
            InsertPreReqs(courseID, preReqs);

            return true;
        }

        public static bool DeleteCourse(string url)
        {
            //Example update url: ?courseID=1&dep=1&courseNumber=101&courseDesc=This+is+a+test&creditHours=3&preReq=1&preReq=2&act=update
            if (url.Length == 0)
            {
                return false;
            }
            var values = HttpUtility.ParseQueryString(url);
            var courseID = values["courseID"];
            if (courseID == null)
            {
                return false;
            }

            // remove any prerequisite association and then remove the course from the course table
            RemoveAllPreReqAssociations(courseID);
            RemoveCourseFromGroup(courseID);
            DeleteCourseFunction(courseID);
            
            return true;
        }

            //Inserts a course into the db
            public static string InsertCourse(string departmentID, string courseDescription, string creditHours, string courseName)
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO course (departmentID, courseDescription, creditHours, courseName) VALUES(@departmentID, @courseDescription, @creditHours, @courseName);";
            command.Parameters.AddWithValue("@departmentID", departmentID);
            command.Parameters.AddWithValue("@courseDescription", courseDescription);
            command.Parameters.AddWithValue("@creditHours", creditHours);
            command.Parameters.AddWithValue("@courseName", courseName);
            command.ExecuteNonQuery();
            long courseID = command.LastInsertedId;
            connection.Close();
            return courseID.ToString();
        }

        //Inserts a course with its group into the CourseGroupCourse table (I didn't name this)
        public static bool InsertCourseWithGroup(string courseID, string groupID)
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO coursegroupcourse (courseID, groupID) VALUES(@courseID, @groupID);";
            command.Parameters.AddWithValue("@courseID", courseID);
            command.Parameters.AddWithValue("@groupID", groupID);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }

        //Inserts a list of Prereq ids into the db with the courseID being the course that has the preReq
        public static bool InsertPreReqs(string courseID, string[] preReqs)
        {
            foreach (string preReq in preReqs)
            {
                using var connection = Connection.GetConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO CoursePreReq (courseID, preReqID) VALUES(@courseID, @preReq);";
                command.Parameters.AddWithValue("@courseID", courseID);
                command.Parameters.AddWithValue("@preReq", preReq);
                command.ExecuteNonQuery();
                connection.Close();
            }

            return true;
        }

        // Removes every entry in prereqs that has courseID as the course or prereqID
        public static bool RemoveAllPreReqAssociations(string courseID)
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM courseprereq WHERE (courseID = @courseID OR preReqID = @courseID) AND courseID<> 0;";
            command.Parameters.AddWithValue("@courseID", courseID);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }

        // Removes prerequisites from the course. Does not change the course being a prerequisite
        public static bool RemoveAllPreReqs(string courseID)
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM courseprereq WHERE courseID = @courseID AND courseID<> 0;";
            command.Parameters.AddWithValue("@courseID", courseID);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }

        //Update the course that has the same courseID with the new information
        public static bool UpdateCourseInfo(string courseID, string departmentID, string courseName, 
            string courseDescription, string creditHours)
        {

            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE course SET  departmentID =  @departmentID,  courseDescription = @courseDescription, creditHours = @creditHours, courseName = @courseName WHERE courseID = @courseID;";
            command.Parameters.AddWithValue("@courseID", courseID);
            command.Parameters.AddWithValue("@departmentID", departmentID);
            command.Parameters.AddWithValue("@courseName", courseName);
            command.Parameters.AddWithValue("@courseDescription", courseDescription);
            command.Parameters.AddWithValue("@creditHours", creditHours);
            command.ExecuteNonQuery();
            connection.Close();

            return true;
        }

            //Return a list of all the group ids and group names
            public static List<string[]> GetCourseGroupNames()
        {
            // Get the connection
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM CourseGroup;";
            //command.ExecuteNonQuery();
            List<string[]> groups = new();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                groups.Add(new string[2] { reader["groupID"].ToString(), reader["groupName"].ToString() });
            }
            reader.Close();
            connection.Close();
            return groups;
        }

        //return a list of all courses in the database
        public static List<string[]> GetAllCourses()
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT courseID, departmentID, courseName FROM Course;";
            List<string[]> courses = new();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                courses.Add(new string[2] { reader["courseID"].ToString(), GetAllDepartmentsIntString()[Int32.Parse(reader["departmentID"].ToString())] + reader["courseName"].ToString() });
            }
            reader.Close();
            connection.Close();
            return courses;
        }

        public static Dictionary<string, int> GetAllDepartments()
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Department;";
            Dictionary<string, int> departments = new();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                departments.Add(reader["departmentAbbr"].ToString(), Int32.Parse(reader["departmentID"].ToString()));
            }
            reader.Close();
            connection.Close();
            return departments;
        }

        public static Dictionary<int, string> GetAllDepartmentsIntString()
        {
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Department;";
            Dictionary<int, string> departments = new();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                departments.Add(Int32.Parse(reader["departmentID"].ToString()), reader["departmentAbbr"].ToString());
            }
            reader.Close();
            connection.Close();
            return departments;
        }

        public static bool DeleteCourseFunction(string courseID)
        {
            if (courseID == null)
            {
                return false;
            }
            //Delete query to sql
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Course WHERE courseID = @courseID AND courseID<> 0;";
            command.Parameters.AddWithValue("@courseID", courseID);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }

        public static bool RemoveCourseFromGroup(string courseID)
        {
            if (courseID == null)
            {
                return false;
            }
            //Delete query to sql
            using var connection = Connection.GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            // need to remove it from the table called semester course because that is where it holds the courses in Plans
            command.CommandText = "DELETE FROM semestercourse WHERE courseID = @courseID AND courseID<> 0; DELETE FROM coursegroupcourse WHERE courseID = @courseID AND courseID<> 0;";
            command.Parameters.AddWithValue("@courseID", courseID);
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
}
