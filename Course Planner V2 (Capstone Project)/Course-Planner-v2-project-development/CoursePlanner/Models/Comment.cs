namespace CoursePlanner.Models
{
    public class Comment
    {
        public int CourseID { get; set; }
        public int PlanID { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public int PersonID { get; set; }
        public string MiamiID { get; set; }
    }
}

