using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoursePlanner.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Auth.OAuth2.Mvc;
using CoursePlanner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Google.Apis.Auth;
using System;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using CoursePlanner.Managers;

namespace CoursePlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                string miamiID = UserController.GetMiamiID(payload);
                PersonManager.GetPersonID(miamiID);
                ViewData["title"] = "Home Page";
                ViewData["pageTitle"] = "Welcome, " + miamiID;
                ViewData["isAdmin"] = PersonManager.GetPerson(miamiID).IsAdmin;
                return View();
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        //[Authorize]
        //public async Task<IActionResult> Home()
        //{
        //    AuthenticateResult auth = await HttpContext.AuthenticateAsync();
        //    string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
        //    try
        //    {
        //        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        //        string miamiID = UserController.GetMiamiID(payload.Email);
        //        ViewData["title"] = "Home Page";
        //        ViewData["pageTitle"] = "Welcome, " + miamiID;
        //        return View();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        //[Authorize]
        //public async Task<IActionResult> ShowTokens()
        //{
        //    // The user is already authenticated, so this call won't trigger authentication.
        //    // But it allows us to access the AuthenticateResult object that we can inspect
        //    // to obtain token related values.
        //    AuthenticateResult auth = await HttpContext.AuthenticateAsync();
        //    string idToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);

        //    string idTokenValid, idTokenIssued, idTokenExpires;
        //    try
        //    {
        //        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        //        idTokenValid = "true";
        //        idTokenIssued = new DateTime(1970, 1, 1).AddSeconds(payload.IssuedAtTimeSeconds.Value).ToString();
        //        idTokenExpires = new DateTime(1970, 1, 1).AddSeconds(payload.ExpirationTimeSeconds.Value).ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        idTokenValid = $"false: {e.Message}";
        //        idTokenIssued = "invalid";
        //        idTokenExpires = "invalid";
        //    }
        //    string accessToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
        //    string refreshToken = auth.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
        //    string accessTokenExpiresAt = auth.Properties.GetTokenValue("expires_at");
        //    string cookieIssuedUtc = auth.Properties.IssuedUtc?.ToString() ?? "<missing>";
        //    string cookieExpiresUtc = auth.Properties.ExpiresUtc?.ToString() ?? "<missing>";
        //    return View();
        //}
    }
}
