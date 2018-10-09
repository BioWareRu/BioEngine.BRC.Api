using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Tag = BioEngine.Core.API.Entities.Tag;

namespace BioEngine.BRC.Api.Controllers
{
    public class TagsController : RestController<Tag, Core.Entities.Tag, int>
    {
        private readonly TagsRepository _repository;

        public TagsController(BaseControllerContext<TagsController> context, TagsRepository repository) : base(context)
        {
            _repository = repository;
        }

        protected override BioRepository<Core.Entities.Tag, int> GetRepository()
        {
            return _repository;
        }

        protected override async Task<Tag> MapRestModel(Core.Entities.Tag domainModel)
        {
            var restModel = await base.MapRestModel(domainModel);
            restModel.Name = domainModel.Name;
            return restModel;
        }

        protected override async Task<Core.Entities.Tag> MapDomainModel(Tag restModel,
            Core.Entities.Tag domainModel = null)
        {
            domainModel = await base.MapDomainModel(restModel, domainModel);
            domainModel.Name = restModel.Name;
            return domainModel;
        }
    }
}