using Microsoft.AspNetCore.Mvc;

namespace CoursePlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        public ApiController()
        {
        }
        // test api call
        [HttpGet("/api/gettest")]
        public object Get()
        {
            return 0;
        }
    }
}
