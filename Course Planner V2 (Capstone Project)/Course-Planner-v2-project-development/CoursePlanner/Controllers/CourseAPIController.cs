using Microsoft.AspNetCore.Mvc;
using CoursePlanner.Models;
using CoursePlanner.Managers;
using System.Collections.Generic;
using System.Text.Json;
// Api controller for all course categories
namespace CoursePlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseAPIController : Controller
    {
        private class CourseResponse
        {
            public List<CourseDetail> courses { get; set; }
        }
        // Gets courses details from database
        [HttpGet("getCourses")]
        public string GetCourses()
        {
            List<CourseDetail> courses = CourseManager.GetAllCourseDetails();

            foreach (CourseDetail cd in courses)
            {
                cd.Prereq = CourseManager.GetAllPreReq(cd.CourseID);
            }

            CourseResponse res = new CourseResponse()
            {
                courses = courses
            };

            return JsonSerializer.Serialize(res);
        }

        private class CourseGroupResponse
        {
            public List<CourseGroup> courseGroups { get; set; }
        }
        // Gets all the different course groups
        [HttpGet("getCourseGroups")]
        public string GetCourseGroups(int degreeID, int reqYear)
        {
            List<CourseGroup> courseGroups = CourseManager.GetCourseGroups(degreeID, reqYear);

            CourseGroupResponse res = new CourseGroupResponse()
            {
                courseGroups = courseGroups
            };

            return JsonSerializer.Serialize(res);
        }
        // add equivalent course
        [HttpPost("/api/equivcourse/add")]
        public object AddEquivalentClass(int courseID, int equivCourseID)
        {
            return CourseManager.AddEquivalentCourse(courseID, equivCourseID);
        }
        // get all equivalent courses
        [HttpGet("/api/equivcourse/get")]
        public object GetEquivalentClass(int courseID)
        {
            return CourseManager.GetEquivalentCourses(courseID);
        }
        // removes equivalent course
        [HttpPost("/api/equivcourse/remove")]
        public object GetEquivalentClass(int courseID, int equivCourseID)
        {
            return CourseManager.RemoveEquivalentCourse(
                courseID, equivCourseID);
        }
        // Gets all the cse core courses in json form from the database
        [HttpGet("/api/csecore")]
        public object GetCSECore()
        {
            return CourseManager.GetCSECore();
        }
        // Gets all the cse elective courses in json form from the database
        [HttpGet("/api/cseelec")]
        public object GetCSEElec()
        {
            return CourseManager.GetCSEElec();
        }
        // Gets all the foundation 3 courses in json form from the database
        [HttpGet("/api/found3")]
        public object GetFound3()
        {
            return CourseManager.GetFound3();
        }
        // Gets all the foundation 2 courses in json form from the database
        [HttpGet("/api/found4")]
        public object GetFound4()
        {
            return CourseManager.GetFound4();
        }
        // Gets all the foundation 5 courses in json form from the database
        [HttpGet("/api/found5")]
        public object GetFound5()
        {
            return CourseManager.GetFound5();
        }
        // Gets all the foundation 2 courses in json form from the database
        [HttpGet("/api/found2")]
        public object GetFound2()
        {
            return CourseManager.GetFound2();
        }
        // Gets all the foundation 1 courses in json form from the database
        [HttpGet("/api/found1")]
        public object GetFound1()
        {
            return CourseManager.GetFound1();
        }
        // Gets all the math/stat electives in json form from the database
        [HttpGet("/api/mathstate")]
        public object GetMathStatE()
        {
            return CourseManager.GetMathStatE();
        }
        // Gets all the computer science core courses in json form from the database
        [HttpGet("/api/compsci")]
        public object GetCompSci()
        {
            return CourseManager.GetCompSci();
        }
        // Gets all the experiential learning courses in json form from the database
        [HttpGet("/api/expl")]
        public object GetExpL()
        {
            return CourseManager.GetExpL();
        }
        // Gets all the intercultural perspectives courses in json form from the database
        [HttpGet("/api/interc")]
        public object GetInterC()
        {
            return CourseManager.GetInterC();
        }
        // Gets all the advanced writing courses in json form from the database
        [HttpGet("/api/advW")]
        public object GetAdvW()
        {
            return CourseManager.GetAdvW();
        }
    }
}
