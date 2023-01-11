using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CoursePlanner.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using CoursePlanner.Managers;
using Google.Apis.Auth;

namespace CoursePlanner.Controllers
{
    // all api calls regarding admin functions are here
    // incomplete as of now since we didn't implement admin functions
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult adminHome()
        {
            var authwait = HttpContext.AuthenticateAsync();
            authwait.Wait();
            AuthenticateResult auth = authwait.Result;
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            var payloadwait = GoogleJsonWebSignature.ValidateAsync(idToken);
            payloadwait.Wait();
            var payload = payloadwait.Result;
            string miamiID = UserController.GetMiamiID(payload);
            if (!PersonManager.GetPerson(miamiID).IsAdmin)
            {
                return Unauthorized();
            }
            ViewData["isAdmin"] = true;
            return View();

        }
        [Authorize]
        public IActionResult adminCourseAdd()
        {
            var authwait = HttpContext.AuthenticateAsync();
            authwait.Wait();
            AuthenticateResult auth = authwait.Result;
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            var payloadwait = GoogleJsonWebSignature.ValidateAsync(idToken);
            payloadwait.Wait();
            var payload = payloadwait.Result;
            string miamiID = UserController.GetMiamiID(payload);
            if (!PersonManager.GetPerson(miamiID).IsAdmin)
            {
                return Unauthorized();
            }
            ViewData["isAdmin"] = true;
            //This sends the url values(course info) into the adminManager
            AdminManager.AddCourse(HttpContext.Request.QueryString.ToString());
            //Read the course groups to put in html
            ViewData["groups"] = AdminManager.GetCourseGroupNames();
            ViewData["allcourses"] = AdminManager.GetAllCourses();
            ViewData["departments"] = AdminManager.GetAllDepartments();
            return View();
        }
    
        [Authorize]
        public IActionResult adminCourseUpdate()
        {
            var authwait = HttpContext.AuthenticateAsync();
            authwait.Wait();
            AuthenticateResult auth = authwait.Result;
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            var payloadwait = GoogleJsonWebSignature.ValidateAsync(idToken);
            payloadwait.Wait();
            var payload = payloadwait.Result;
            string miamiID = UserController.GetMiamiID(payload);
            if (!PersonManager.GetPerson(miamiID).IsAdmin)
            {
                return Unauthorized();
            }
            ViewData["isAdmin"] = true;
            AdminManager.UpdateCourse(HttpContext.Request.QueryString.ToString());
            ViewData["departments"] = AdminManager.GetAllDepartments();
            ViewData["allcourses"] = AdminManager.GetAllCourses();
            return View();
            
        }

        [Authorize]
        public IActionResult adminDeleteCourse()
        {
            var authwait = HttpContext.AuthenticateAsync();
            authwait.Wait();
            AuthenticateResult auth = authwait.Result;
            // Get the idToken
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            var payloadwait = GoogleJsonWebSignature.ValidateAsync(idToken);
            payloadwait.Wait();
            var payload = payloadwait.Result;
            string miamiID = UserController.GetMiamiID(payload);
            if (!PersonManager.GetPerson(miamiID).IsAdmin)
            {
                return Unauthorized();
            }
            ViewData["isAdmin"] = true;
            AdminManager.DeleteCourse(HttpContext.Request.QueryString.ToString());
            ViewData["allcourses"] = AdminManager.GetAllCourses();
            return View();
        }
        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
