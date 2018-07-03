using BioEngine.Core.API;
using BioEngine.Core.Entities;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class ContentController : ContentController<ContentItem, int>
    {
        private readonly ContentRepository _repository;

        public ContentController(BaseControllerContext<ContentController> context, ContentRepository repository) : base(context)
        {
            _repository = repository;
        }

        protected override ContentItem MapEntity(ContentItem entity, ContentItem newData)
        {
            return entity;
        }

        protected override BioRepository<ContentItem, int> GetRepository()
        {
            return _repository;
        }
    }
}