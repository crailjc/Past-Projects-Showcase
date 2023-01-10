using System;

namespace CoursePlanner.Models
{
    public class PlanDetail
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public int EnrollYear { get; set; }
        public string EnrollSeason { get; set; }
        public int PlanLength { get; set; }
        public string DegreeName { get; set; }
    }
}
