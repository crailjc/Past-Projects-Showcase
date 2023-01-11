using System;
namespace CoursePlanner.Models
{
    public class Degree
    {
        public int DegreeID { get; set; }
        public string DegreeName { get; set; }
    }

    public class DegreeRequirements
    {
        public int DegreeID { get; set; }
        public int GroupID { get; set; }
        public int Year { get; set; }
        public int MinCreditHours { get; set; }
        public int MaxCreditHours { get; set; }
    }
}
