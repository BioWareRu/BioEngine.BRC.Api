using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Post = BioEngine.BRC.Api.Entities.Post;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ContentController<Post, Domain.Entities.Post, int, PostData>
    {
        private readonly PostsRepository _postsRepository;

        public PostsController(BaseControllerContext<PostsController> context, PostsRepository postsRepository) :
            base(context)
        {
            _postsRepository = postsRepository;
        }

        protected override BioRepository<Domain.Entities.Post, int> GetRepository()
        {
            return _postsRepository;
        }
    }
}