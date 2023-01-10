using System;
using System.Collections.Generic;

namespace CoursePlanner.Models
{
    // creating data model objects to be able to access attributes from the objects
    public class Course
    {
        public int CourseID { get; set; }
        public int DepartmentID { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public int CreditHours { get; set; }
    }
    public class CourseGroupCourse
    {
        public int CourseID { get; set; }
        public int GroupID { get; set; }
    }
    // course group object used to hold the id and name of a group object for display
    public class CourseGroup : IComparable<CourseGroup>
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }

        public int CompareTo(CourseGroup other)
        {
            return GroupID - other.GroupID;
        }
    }
    // pre req object that holds both course ids
    public class CoursePreReq
    {
        public int CourseID { get; set; }
        public int PreReqCourseID { get; set; }
    }
    // department obj with an ID and abbreviation
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentAbbr { get; set; }
    }
    // course object with all of the info needed for the course i.e. what department it is, num of credit hours etc.
    public class CourseDetail
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int GroupID { get; set; }
        public string CourseGroup { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public List<string> Prereq { get; set; }
        // splices the prereq table results to split the ids out
        public string formatPrereqs()
        {
            var str = "[";
            for (var i = 0; i < Prereq.Count; i++)
            {
                str += "[";
                str += Prereq[i];
                str += "]";
                if (i+1 != Prereq.Count)
                {
                    str += ",";
                }
            }
            str += "]";
            return str;
        }
    }

}
