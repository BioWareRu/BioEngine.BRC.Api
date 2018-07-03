using BioEngine.Core.API;
using BioEngine.Core.Entities;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class SitesController : RestController<Site, int>
    {
        private readonly SitesRepository _sitesRepository;

        public SitesController(BaseControllerContext<SitesController> context, SitesRepository sitesRepository) :
            base(context)
        {
            _sitesRepository = sitesRepository;
        }

        protected override Site MapEntity(Site entity, Site newData)
        {
            entity.Title = newData.Title;
            entity.Url = newData.Url;
            entity.Description = newData.Description;
            entity.Keywords = newData.Keywords;
            return entity;
        }

        protected override BioRepository<Site, int> GetRepository()
        {
            return _sitesRepository;
        }
    }
}