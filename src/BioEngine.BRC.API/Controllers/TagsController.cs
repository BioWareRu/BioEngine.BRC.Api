using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Tag = BioEngine.Core.API.Entities.Tag;

namespace BioEngine.BRC.Api.Controllers
{
    public class TagsController : RestController<Tag, Core.Entities.Tag, int>
    {
        public TagsController(BaseControllerContext<Core.Entities.Tag, int> context) : base(context)
        {
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