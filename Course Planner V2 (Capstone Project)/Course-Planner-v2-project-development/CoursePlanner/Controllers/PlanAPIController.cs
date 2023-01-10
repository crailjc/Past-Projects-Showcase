using Microsoft.AspNetCore.Mvc;
using CoursePlanner.Models;
using CoursePlanner.Managers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

namespace CoursePlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanApiController : ControllerBase
    {

        private class HttpResponse
        {
            public string status { get; set; }
            public string message { get; set; }
        }

        [HttpGet("semester/remove")]
        [Authorize]
        public async Task<bool> RemoveSemesterIDs(int planID, string semIDs)
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
                var personID = PersonManager.GetPersonID(miamiID);
                // Check if miamiID owns this plan
                if (!PlanManager.PersonOwnsPlan(personID, planID))
                {
                    // Throw an exception
                    throw PlanManager.NotYourPlanIDException();
                }
                string[] ids = semIDs.Split(",");
                bool success = true;
                foreach (var id in ids)
                {
                    if (int.TryParse(id, out int semID))
                    {

                        if (!PlanManager.RemoveSemesterFromPlan(planID, semID))
                        {
                            success = false;
                        }
                    } else
                    {
                        success = false;
                    }

                }
                return success;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("semester/add")]
        [Authorize]
        public async Task<bool> AddNextSemesterIDs(int planID)
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
                var personID = PersonManager.GetPersonID(miamiID);
                // Check if miamiID owns this plan
                if (!PlanManager.PersonOwnsPlan(personID, planID))
                {
                    // Throw an exception
                    throw PlanManager.NotYourPlanIDException();
                }
                List<DetailedPlan> plan = PlanManager.GetDetailedPlans(planID);
                int year = plan[0].CurrentYear;
                foreach (var p in plan)
                {
                    year = Math.Max(year, p.CurrentYear);
                }
                year /= 100;
                string[,] semesters = GenerateSemesters(year + 1, 1);
                InsertSemesterPlan(planID, semesters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("savePlan")]
        public string SavePlan([FromBody] PlanSemesters planSemesters)
        {
            HttpResponse res = new HttpResponse()
            {
                status = "OK",
                message = "Plan saved"
            };

            List<SemesterCourses> semesterCourses = planSemesters.semesters;

            foreach (SemesterCourses semester in semesterCourses)
            {
                int semesterID = semester.semesterID;

                PlanManager.DeleteSemesterCourse(semesterID);

                foreach (int courseID in semester.courses)
                {
                    PlanManager.InsertSemesterCourse(semesterID, courseID);
                }
            }

            return JsonSerializer.Serialize(res);
        }

        /**
         * Method to handle incoming HTTP POST request to insert a plan into a user's record.
         */
        [HttpPost("createPlan")]
        public string StorePlan([FromBody]PostPlan plan)
        {
            HttpResponse response;

            // Parse information from POST body.
            string miamiID = plan.miamiID;
            string planName = plan.planName;
            int enrollYear = Int32.Parse(plan.enrollYear);
            string enrollSeason = plan.enrollSeason;
            int planLength = Int32.Parse(plan.planLength);
            int degreeID = Int32.Parse(plan.degreeID);

            // Return HTTP response with failure information if plan name already exists.
            if(PlanManager.PlanNameExist(miamiID, planName))
            {
                response = new HttpResponse()
                {
                    status = "FAIL",
                    message = "Plan name exists!"
                };
                return JsonSerializer.Serialize(response);
            }

            // Get the plan ID of the inserted plan.
            int planID = InsertPlan(miamiID, planName, enrollYear, enrollSeason);
            // Generate semesters based on user's enroll year season and desired length of plan.
            string[,] semesters = GenerateSemesters(enrollYear, planLength);

            // Also insert into DegreePlan and SemesterPlan table using plan ID.
            InsertDegreePlan(planID, degreeID);
            InsertSemesterPlan(planID, semesters);

            // Return success HTTP response after a plan is stored.
            response = new HttpResponse()
            {
                status = "OK",
                message = planID.ToString()
            };

            return JsonSerializer.Serialize(response);
        }

        /**
         * Helper method to insert into Plan table in database and return the new inserted plan ID.
         */
        private int InsertPlan(string miamiID, string planName, int enrollYear, string enrollSeason)
        {
            int personID = PersonManager.GetPersonID(miamiID);
            // Create a new plan and add the information
            var plan = new Plan() {
                PersonID = personID,
                PlanName = planName,
                EnrollYear = enrollYear,
                EnrollSeason = enrollSeason
            };
            // Insert the plan and return the plan ID if it was successful
            return PlanManager.InsertNewPlan(plan);
        }

        /**
         * Helper method to insert into DegreePlan table in database and return the insertion status (true or false).
         */
        private bool InsertDegreePlan(int planID, int degreeID)
        {
            var degreePlan = new DegreePlan()
            {
                PlanID = planID,
                DegreeID = degreeID
            };
            return PlanManager.InsertNewDegreePlan(degreePlan);
        }

        /**
         * Helper method to insert into SemesterPlan table in database and return the insertion status (true or false).
         */
        private bool InsertSemesterPlan(int planID, string[,] semesters)
        {
            bool ret = true;

            for(int i = 0; i < semesters.GetLength(0); i++)
            {
                SemesterPlan semesPlan1 = new SemesterPlan()
                {
                    PlanID = planID,
                    CurrentYear = semesters[i, 0],
                    Season = semesters[i, 1]
                };
                SemesterPlan semesPlan2 = new SemesterPlan()
                {
                    PlanID = planID,
                    CurrentYear = semesters[i, 2],
                    Season = semesters[i, 3]
                };
                SemesterPlan semesPlan3 = new SemesterPlan()
                {
                    PlanID = planID,
                    CurrentYear = semesters[i, 4],
                    Season = semesters[i, 5]
                };
                SemesterPlan semesPlan4 = new SemesterPlan()
                {
                    PlanID = planID,
                    CurrentYear = semesters[i, 6],
                    Season = semesters[i, 7]
                };

                ret = PlanManager.InsertNewSemesterPlan(semesPlan1);
                ret = PlanManager.InsertNewSemesterPlan(semesPlan2);
                ret = PlanManager.InsertNewSemesterPlan(semesPlan3);
                ret = PlanManager.InsertNewSemesterPlan(semesPlan4);
            }

            SemesterPlan completed = new SemesterPlan()
            {
                PlanID = planID,
                CurrentYear = semesters[0, 0],
                Season = "Completed"
            };

            ret = PlanManager.InsertNewSemesterPlan(completed);

            return ret;
        }

        /**
         * Helper method to generate semesters based on user's enroll year season and desired length of plan.
         */
        private string[,] GenerateSemesters(int enrollYear, int planLength)
        {
            int length = planLength;
            int year = enrollYear;
            string[,] semesters = new string[planLength, 8];

            for (int i = 0; i < length; i++)
            {
                semesters[i, 0] = ConvertToCurrentYear(year, "Fall");
                semesters[i, 1] = "Fall";
                semesters[i, 2] = ConvertToCurrentYear(year, "Winter");
                semesters[i, 3] = "Winter";

                year++;

                semesters[i, 4] = ConvertToCurrentYear(year, "Spring");
                semesters[i, 5] = "Spring";
                semesters[i, 6] = ConvertToCurrentYear(year, "Summer");
                semesters[i, 7] = "Summer";

                
            }

            return semesters;
        }

        /**
         * Helper method to convert a year and season into the format of, for example, 2122Spring.
         */
        private string ConvertToCurrentYear(int year, string season)
        {
            if(season == "Fall" || season == "Winter")
            {
                return year.ToString() + (year % 100 + 1).ToString();
            } else
            {
                return (year - 1).ToString() + (year % 100).ToString();
            }
        }

        [HttpGet("/api/plans/detailed")]
        public List<DetailedPlan> GetPlanInformation(int planID)
        {
            // Get the detailed plans from the given planID
            return PlanManager.GetDetailedPlans(planID);
        }

        [HttpGet("/api/plans/course/add")]
        public bool AddCourseToPlan(int semesterID,
            int courseID)
        {
            // Attempt to add the course to the semester
            return PlanManager.AddCourseToSemester(
                semesterID, courseID);
        }

        [HttpGet("/api/plans/course/remove")]
        public bool RemoveCourseFromPlan(int semesterID,
            int courseID)
        {
            // Remove the coure from the semester
            return PlanManager.RemoveCourseFromSemester(
                semesterID, courseID);
        }

        [HttpGet("/api/plans/semester/add")]
        public int AddSemesterToPlan(int planID,
            string season, int year)
        {
            // Addd the semester to the plan
            return PlanManager.AddSemesterToPlan(
                planID, season, year);
        }

        [HttpGet("/api/plans/semester/remove")]
        public bool RemoveSemesterFromPlan(int planID,
            int semesterID)
        {
            // Remove the semester to the plan
            return PlanManager.RemoveSemesterFromPlan(
                planID, semesterID);
        }

    }
}
