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
        private readonly ICurrentUserProvider<string> _currentUserProvider;

        public UserController(BaseControllerContext context, ICurrentUserProvider<string> currentUserProvider) :
            base(context)
        {
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet("/v1/me")]
        public ActionResult<IUser<string>> Me()
        {
            return Ok(_currentUserProvider.CurrentUser);
        }
    }
}
