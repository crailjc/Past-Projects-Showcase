using System.Collections.Generic;

namespace CoursePlanner.Models
{
    public class Plan
    {
        public int PlanID { get; set; }
        public int PersonID { get; set; }
        public string PlanName { get; set; }
        public int EnrollYear { get; set; }
        public string EnrollSeason { get; set; }

    }

    public class PostPlan
    {
        public string miamiID { get; set; }
        public string planName { get; set; }
        public string enrollYear { get; set; }
        public string enrollSeason { get; set; }
        public string planLength { get; set; }
        public string degreeID { get; set; }
    }

    public class DegreePlan
    {
        public int PlanID { get; set; }
        public int DegreeID { get; set; }

    }

    public class SemesterPlan
    {
        public int PlanID { get; set; }
        public string Season { get; set; }
        public string CurrentYear { get; set; }

    }

    public class SemesterCourses
    {
        public int semesterID { get; set; }
        public List<int> courses { get; set; }
    }

    public class PlanSemesters
    {
        public string miamiID { get; set; }
        public List<SemesterCourses> semesters { get; set; }
    }
}
