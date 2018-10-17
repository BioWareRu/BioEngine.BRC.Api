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

        protected override async Task<Tag> MapRestModelAsync(Core.Entities.Tag domainModel)
        {
            var restModel = await base.MapRestModelAsync(domainModel);
            restModel.Name = domainModel.Name;
            return restModel;
        }

        protected override async Task<Core.Entities.Tag> MapDomainModelAsync(Tag restModel,
            Core.Entities.Tag domainModel = null)
        {
            domainModel = await base.MapDomainModelAsync(restModel, domainModel);
            domainModel.Name = restModel.Name;
            return domainModel;
        }
    }
}