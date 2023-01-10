using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CoursePlanner.Managers;
using CoursePlanner.Models;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CoursePlanner.Controllers
{
    // user controller to process logins
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        // Generating a new id token when a user logs in to the website
        public async Task<IActionResult> Login(string idToken)
        {
            
            Payload payload;
            try
            {
                payload = await ValidateAsync(idToken, new ValidationSettings
                {
                    Audience = new[] { "814089137112-gvdprgadav0ef7dv476r1pdutba8hn41.apps.googleusercontent.com" }
                });
            }
            catch
            {
                return BadRequest(new { msg = "Invalid Token" });
            }

            string miamiID = payload.Email.Split("@", StringSplitOptions.RemoveEmptyEntries)[0];

            HttpContext.Session.SetString("miamiID", miamiID);


            if (miamiID != "" && !PersonManager.ContainsPerson(miamiID))
            {
                // Inserts person if not in the database already
                PersonManager.InsertPerson(miamiID);
            }
            

            // returns the user to home on successful log in
            return RedirectToAction("Home", "Home");

        }
        // Getting user credntials on log in
        public async Task<Payload> GetUserCredentials()
        {
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return payload;
            }
            catch
            {
                return null;
            }
        }
        // Grabbing the miami ID to display from the user token
        public static string GetMiamiID(Payload payload)
        {
            string email = payload.Email;
            return email.Split("@", StringSplitOptions.RemoveEmptyEntries)[0];
        }
        // Getting the person ID from the database
        public static int getPersonID(Payload payload)
        {
            string miamiID = GetMiamiID(payload);
            return PersonManager.GetPersonID(miamiID);
        }
    }
}
