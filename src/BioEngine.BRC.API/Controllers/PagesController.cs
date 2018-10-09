using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Page = BioEngine.Core.API.Entities.Page;

namespace BioEngine.BRC.Api.Controllers
{
    public class PagesController : RestController<Page, Core.Entities.Page, int>
    {
        private readonly PagesRepository _pagesRepository;

        public PagesController(BaseControllerContext<PagesController> context, PagesRepository pagesRepository) :
            base(context)
        {
            _pagesRepository = pagesRepository;
        }

        protected override BioRepository<Core.Entities.Page, int> GetRepository()
        {
            return _pagesRepository;
        }

        protected override async Task<Core.Entities.Page> MapDomainModel(Page restModel, Core.Entities.Page domainModel = null)
        {
            domainModel = await base.MapDomainModel(restModel, domainModel);
            domainModel.Title = restModel.Title;
            domainModel.Url = restModel.Url;
            domainModel.SiteIds = restModel.SiteIds;
            domainModel.Text = restModel.Text;
            return domainModel;
        }

        protected override async Task<Page> MapRestModel(Core.Entities.Page domainModel)
        {
            var restModel = await base.MapRestModel(domainModel);
            restModel.Title = domainModel.Title;
            restModel.Url = domainModel.Url;
            restModel.SiteIds = domainModel.SiteIds;
            restModel.Text = domainModel.Text;
            return restModel;
        }
    }
}