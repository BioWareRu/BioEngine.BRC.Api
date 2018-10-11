using BioEngine.Core.API;
using BioEngine.Core.API.Entities;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class ContentController : ContentController<ContentEntityItem, Core.Entities.ContentItem, int>
    {
        private readonly ContentRepository _repository;

        public ContentController(BaseControllerContext<ContentController> context, ContentRepository repository) :
            base(context)
        {
            _repository = repository;
        }

        protected override BioRepository<Core.Entities.ContentItem, int> GetRepository()
        {
            return _repository;
        }
    }
}