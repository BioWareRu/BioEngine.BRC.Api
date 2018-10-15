using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Post = BioEngine.BRC.Api.Entities.Post;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ContentController<Post, Domain.Entities.Post, int, PostData>
    {
        public PostsController(BaseControllerContext<Domain.Entities.Post, int> context) : base(context)
        {
        }
    }
}