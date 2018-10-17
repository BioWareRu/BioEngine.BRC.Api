using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Site = BioEngine.Core.API.Entities.Site;

namespace BioEngine.BRC.Api.Controllers
{
    public class SitesController : RestController<Site, Core.Entities.Site, int>
    {
        public SitesController(BaseControllerContext<Core.Entities.Site, int> context) : base(context)
        {
        }

        protected override async Task<Site> MapRestModelAsync(Core.Entities.Site domainModel)
        {
            var restModel = await base.MapRestModelAsync(domainModel);
            restModel.Title = domainModel.Title;
            restModel.Url = domainModel.Url;
            return restModel;
        }

        protected override async Task<Core.Entities.Site> MapDomainModelAsync(Site restModel,
            Core.Entities.Site domainModel = null)
        {
            domainModel = await base.MapDomainModelAsync(restModel, domainModel);
            domainModel.Title = restModel.Title;
            domainModel.Url = restModel.Url;
            return domainModel;
        }
    }
}