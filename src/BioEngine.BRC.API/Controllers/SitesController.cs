using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Site = BioEngine.Core.API.Entities.Site;

namespace BioEngine.BRC.Api.Controllers
{
    public class SitesController : RestController<Site, Core.Entities.Site, int>
    {
        private readonly SitesRepository _sitesRepository;

        public SitesController(BaseControllerContext<SitesController> context, SitesRepository sitesRepository) :
            base(context)
        {
            _sitesRepository = sitesRepository;
        }

        protected override BioRepository<Core.Entities.Site, int> GetRepository()
        {
            return _sitesRepository;
        }

        protected override async Task<Site> MapRestModel(Core.Entities.Site domainModel)
        {
            var restModel = await base.MapRestModel(domainModel);
            restModel.Title = domainModel.Title;
            restModel.Url = domainModel.Url;
            return restModel;
        }

        protected override async Task<Core.Entities.Site> MapDomainModel(Site restModel, Core.Entities.Site domainModel = null)
        {
            domainModel = await base.MapDomainModel(restModel, domainModel);
            domainModel.Title = restModel.Title;
            domainModel.Url = restModel.Url;
            return domainModel;
        }
    }
}