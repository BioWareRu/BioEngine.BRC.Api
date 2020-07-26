using BioEngine.BRC.Common.Users;
using BioEngine.BRC.Common.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    public class UserController : BaseController
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public UserController(BaseControllerContext context, ICurrentUserProvider currentUserProvider) :
            base(context)
        {
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet("/v1/me")]
        public ActionResult<IUser> Me()
        {
            return Ok(_currentUserProvider.CurrentUser);
        }
    }
}
