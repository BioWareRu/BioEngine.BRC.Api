using BioEngine.Core.API;
using BioEngine.Core.Entities;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class TagsController : RestController<Tag, int>
    {
        private readonly TagsRepository _repository;

        public TagsController(BaseControllerContext<TagsController> context, TagsRepository repository) : base(context)
        {
            _repository = repository;
        }

        protected override Tag MapEntity(Tag entity, Tag newData)
        {
            entity.Name = newData.Name;
            return entity;
        }

        protected override BioRepository<Tag, int> GetRepository()
        {
            return _repository;
        }
    }
}