using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using CoursePlanner.Models;
using CoursePlanner.Mangers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Google.Apis.Auth;
using CoursePlanner.Managers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CoursePlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        [HttpGet]
        public List<Comment> GetComments(int PlanID)
        {
            return CommentManager.GetComments(PlanID);
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> AddCommentAsync(int PlanID, string content)
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
                if (!PlanManager.PersonOwnsPlan(personID, PlanID))
                {
                    // Throw an exception
                    throw PlanManager.NotYourPlanIDException();
                }
                // Create comment
                var comment = new Comment
                {
                    PlanID = PlanID,
                    Text = content,
                    PersonID = personID
                };
                // Add new comment
                var result = CommentManager.AddComment(comment);
                return result;
            }
            catch
            {
                return false;
            }
            
        }
    }
}