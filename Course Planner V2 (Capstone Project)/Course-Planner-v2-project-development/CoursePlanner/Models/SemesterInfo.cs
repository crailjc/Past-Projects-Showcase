namespace CoursePlanner.Models
{
    public class SemesterInfo
    {
        // Store the ID of the current semester
        public int SemesterID { get; set; }
        // Store the plan models
        public EditPlanModel Plan { get; set; }

        public SemesterInfo()
        {
            // Create an initial plan
            Plan = new();
        }

        public int GetTotalCredits()
        {
            var sum = 0;
            foreach(DetailedCourse c in Plan.courses)
            {
                sum += c.Course.CreditHours;
            }
            return sum;
        }
    }
}
