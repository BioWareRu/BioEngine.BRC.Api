using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ContentController<Post, int, PostData, Entities.Response.Post, Entities.Request.PostRequestItem>
    {
        public PostsController(BaseControllerContext<Post, int> context) : base(context)
        {
        }
    }
}