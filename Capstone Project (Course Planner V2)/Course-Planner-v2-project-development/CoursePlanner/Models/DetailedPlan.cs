using System;
namespace CoursePlanner.Models
{
    public class DetailedPlan
    {
        public int PlanID { get; set; }
        public int SemesterID { get; set; }
        public string Season { get; set; }
        public int CurrentYear { get; set; }
        public int CourseID { get; set; }
        public int DepartmentID { get; set; }
        public string CourseDescription { get; set; }
        public int CreditHours { get; set; }
        public string CourseName { get; set; }
        public string DepartmentAbbr { get; set; }

        public DetailedCourse ToCourse()
        {
            // Create a course object
            DetailedCourse course = new();
            // Create a new course object
            course.Course = new();
            // Set the course information
            course.Course.CourseID = CourseID;
            course.Course.CourseName = CourseName;
            course.Course.CreditHours = CreditHours;
            course.Course.DepartmentID = DepartmentID;
            course.Course.Description = CourseDescription;
            // Set the course department abbreviation
            course.DepartmentAbbr = DepartmentAbbr;
            // Return the course
            return course;
        }
    }
}
