using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ContentController<Post, int>
    {
        private readonly PostsRepository _postsRepository;

        public PostsController(BaseControllerContext<PostsController> context, PostsRepository postsRepository) :
            base(context)
        {
            _postsRepository = postsRepository;
        }

        protected override BioRepository<Post, int> GetRepository()
        {
            return _postsRepository;
        }

        protected override Post MapEntity(Post entity, Post newData)
        {
            entity = MapContentData(entity, newData);
            entity.Data = newData.Data;
            return entity;
        }
    }
}