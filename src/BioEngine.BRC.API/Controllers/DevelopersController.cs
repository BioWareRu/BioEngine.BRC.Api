using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class DevelopersController : SectionController<Developer, int>
    {
        private readonly DevelopersRepository _developersRepository;

        public DevelopersController(BaseControllerContext<DevelopersController> context,
            DevelopersRepository developersRepository) :
            base(context)
        {
            _developersRepository = developersRepository;
        }

        protected override Developer MapEntity(Developer entity, Developer newData)
        {
            entity = MapSectionData(entity, newData);
            entity.Data = newData.Data;
            return entity;
        }

        protected override BioRepository<Developer, int> GetRepository()
        {
            return _developersRepository;
        }

        protected override string GetUploadPath()
        {
            return "developers";
        }
    }
}