using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.API.Entities;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class MenuController : RestController<Menu, Core.Entities.Menu, int>
    {
        public MenuController(BaseControllerContext<Core.Entities.Menu, int> context) : base(context)
        {
        }

        protected override async Task<Menu> MapRestModelAsync(Core.Entities.Menu domainModel)
        {
            var restModel = await base.MapRestModelAsync(domainModel);
            restModel.Items = domainModel.Items;
            restModel.Title = domainModel.Title;
            restModel.SiteIds = domainModel.SiteIds;
            return restModel;
        }

        protected override async Task<Core.Entities.Menu> MapDomainModelAsync(Menu restModel, Core.Entities.Menu domainModel = null)
        {
            domainModel = await base.MapDomainModelAsync(restModel, domainModel);
            domainModel.Items = restModel.Items;
            domainModel.Title = restModel.Title;
            domainModel.SiteIds = restModel.SiteIds;
            return domainModel;
        }
    }
}