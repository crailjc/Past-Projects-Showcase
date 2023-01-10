using System;
namespace CoursePlanner.Models
{
    // Creates a data model for a course to be able to grab certain attributes from a course
    public class DetailedCourse
    {   
        public Course Course { get; set; }
        // adding course ID to be able to retrieve
        public int CourseID { get; set; }
        // adding department Abbreviation for use
        public string DepartmentAbbr { get; set; }
        // course number i.e. cse 174, 174
        public string CourseName { get; set; }
        // credit hour attribute
        public string CreditHours { get; set; }
    }
}
