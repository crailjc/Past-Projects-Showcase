using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoursePlanner.Models;
using Microsoft.AspNetCore.Http;
using CoursePlanner.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Google.Apis.Auth;

namespace CoursePlanner.Controllers
{
    public class PlanController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public PlanController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> EditPlanAsync(int planID)
        {
            // Get the authentication information 
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            try
            {
                // Get the payload
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                // Get the miamiID from the payload
                string miamiID = UserController.GetMiamiID(payload);
                // Set the miamiID
                ViewData["miamiID"] = miamiID;
                var personID = PersonManager.GetPersonID(miamiID);
                ViewData["isAdmin"] = PersonManager.GetPerson(miamiID).IsAdmin;
                // Check if miamiID owns this plan
                if (!PlanManager.PersonOwnsPlan(personID, planID))
                {
                    // Throw an exception
                    throw PlanManager.NotYourPlanIDException();
                }
            }
            catch
            {
                return BadRequest();
            }
            // Get the detailed plan
            var detailed = PlanManager.GetDetailedPlans(planID);
            // Store the plans in the dictionary
            var organized = new SortedDictionary<string,
                SortedDictionary<string, SemesterInfo>>();
            // Store all of the course IDs that are saved
            List<int> courses = new();
            // Iterate through all of the courses
            foreach(var detail in detailed)
            {
                // Get the current year
                var year = detail.CurrentYear;
                // Format the year as xxxx-xx (ex. 2020-21)
                var temp = year % 100;
                string formalYear = year / 100 + "-" + temp;
                if (detail.Season == "Completed")
                {
                    ViewData["CompSemID"] = detail.SemesterID;
                    formalYear = "Completed";
                }
                // Get the current model in that year, or
                // create a new dictionary
                SortedDictionary<string, SemesterInfo> plans =
                    organized.GetValueOrDefault(formalYear, new SortedDictionary<string,
                    SemesterInfo>());
                // Check if the season (fall, spring, ...) exists
                if (plans.ContainsKey(detail.Season))
                {
                    // Add the plan to the existing model
                    plans[detail.Season].Plan.AddPlan(detail);
                }
                else
                {
                    // Create a new model
                    var plan = new SemesterInfo();
                    // Add the information to the course 
                    plan.Plan.AddPlan(detail);
                    // Set the semester ID
                    plan.SemesterID = detail.SemesterID;
                    // Add the model to the dictionary
                    plans.Add(detail.Season, plan);
                }
                // Add the course to the total course list
                var course = detail.ToCourse();
                courses.Add(course.Course.CourseID);
                // Add the dictionary to the organized dictionary
                organized[formalYear] = plans;
            }
            // Store the organized semesters in the view data
            ViewData["organized"] = organized;
            // Get the comments and store it in the viewdata
            ViewData["comments"] = new CommentModel(planID);
            // Store the planID
            ViewData["planID"] = planID;
            // Get the year information of the plan and add it to the ViewData
            int planYear = PlanManager.GetPlanYear(planID);
            var degreeID = PlanManager.GetDegreeID(planID);
             // Set the courses
            ViewData["courses"] = courses;
            // Get all of the groups
            SortedDictionary<CourseGroup, List<CourseDetail>> groups = new();
            foreach (var Cgroup in CourseManager.GetCourseGroups(degreeID, planYear))
            {
                // Get all of the courses for the group
                groups[Cgroup] = CourseManager.GetDetailedCousesForGroup(Cgroup.GroupID);
            }
            // Store the groups in the ViewData
            ViewData["groups"] = groups;
            return View();
        }

        /**
         * Method to return the view of create plan page.
         */
        [Authorize]
        public async Task<IActionResult> CreatePlanAsync()
        {
            // Get the authentication information 
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            try
            {
                // Get the payload
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                // Get the miamiID from the payload
                string miamiID = UserController.GetMiamiID(payload);
                PersonManager.GetPersonID(miamiID);
                ViewData["isAdmin"] = PersonManager.GetPerson(miamiID).IsAdmin;
                // Set the miamiID
                ViewData["miamiID"] = miamiID;
                ViewData["title"] = "Create Plan";
                ViewData["pageTitle"] = "Create a Plan";
                ViewData["years"] = GetYears();
                ViewData["degrees"] = DegreeManager.GetAllDegrees();
                return View();
            }
            catch
            {
                return BadRequest();
            }
        }

        /**
         * Helper method to get the previous ten years as an array,
         * counting from current year.
         */
        private int[] GetYears()
        {
            int[] years = new int[10];
            int currentYear = System.DateTime.Now.Year;

            for(int i = 0; i < 10; i++)
            {
                years[i] = currentYear - i;
            }

            return years;
        }

        /**
         * Method to return the view of select plan page.
         * The miami ID here is just a placeholder, it should be replaced with logged in user's miami ID 
         * later when CAS login is implemented. 
         */
        public async Task<IActionResult> SelectPlan()
        {
            // Get the authentication information 
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            try
            {
                // Get the payload
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                // Get the miamiID from the payload
                string miamiID = UserController.GetMiamiID(payload);
                PersonManager.GetPersonID(miamiID);
                // Set the miamiID
                ViewData["miamiID"] = miamiID;
                ViewData["plans"] = PlanManager.GetPlanDetails(miamiID);
                ViewData["title"] = "Select Plan";
                ViewData["pageTitle"] = "Select a Plan";
                ViewData["isAdmin"] = PersonManager.GetPerson(miamiID).IsAdmin;
                return View();
            }
            catch
            {
                return BadRequest();
            }
        }

        /**
         * Method to delete a plan and redirect to select plan page.
         */
        [Authorize]
        public async Task<IActionResult> DeletePlanAsync(int planID)
        {
            // Get the authentication information 
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            try
            {
                // Get the payload
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                // Get the miamiID from the payload
                string miamiID = UserController.GetMiamiID(payload);
                int personID = PersonManager.GetPersonID(miamiID);
                if (PlanManager.PersonOwnsPlan(personID, planID))
                {
                    PlanManager.DeletePlan(planID);
                    return RedirectToAction("SelectPlan");
                } else
                {
                    throw PlanManager.NotYourPlanIDException();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                    HttpContext.TraceIdentifier
            });
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}