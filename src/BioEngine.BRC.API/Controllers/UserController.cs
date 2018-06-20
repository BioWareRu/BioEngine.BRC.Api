using BioEngine.Core.API;
using BioEngine.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    public class UserController : BaseController
    {
        public UserController(BaseControllerContext<UserController> context) : base(context)
        {
        }

        [HttpGet("/v1/me")]
        public ActionResult<IUser> Me()
        {
            return Ok(CurrentUser);
        }
    }
}