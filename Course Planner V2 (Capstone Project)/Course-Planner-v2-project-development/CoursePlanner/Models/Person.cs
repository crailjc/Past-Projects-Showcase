using System;
namespace CoursePlanner.Models
{
    // Person object to be used to store attributes to a given person
    // i.e. attaching miamiID to a person object
    public class Person
    {
        public int PersonID { get; set; }
        public string MiamiID { get; set; }
        // processes if a user is an admin or not
        public bool IsAdmin { get; set; }
        // processes if a user is an advisor or not
        public bool IsAdvisor { get; set; }
    }
}
