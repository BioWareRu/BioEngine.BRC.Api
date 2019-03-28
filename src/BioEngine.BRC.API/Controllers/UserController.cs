using BioEngine.Core.Users;
using BioEngine.Core.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    public class UserController : BaseController
    {
        public UserController(BaseControllerContext context) : base(context)
        {
        }

        [HttpGet("/v1/me")]
        public ActionResult<IUser> Me()
        {
            return Ok(CurrentUser);
        }
    }
}