using System.Collections.Generic;

namespace CoursePlanner.Models
{
    public class EditPlanModel
    {

        public List<DetailedPlan> plans = new();
        public List<DetailedCourse> courses = new();

        public void AddPlan(DetailedPlan plan)
        {
            // Add the course
            courses.Add(plan.ToCourse());
            // Add the plan
            plans.Add(plan);
        }
    }
}
