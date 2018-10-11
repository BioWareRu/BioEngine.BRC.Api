using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Developer = BioEngine.BRC.Api.Entities.Developer;

namespace BioEngine.BRC.Api.Controllers
{
    public class DevelopersController : SectionController<Developer, Domain.Entities.Developer, int, DeveloperData>
    {
        private readonly DevelopersRepository _developersRepository;

        public DevelopersController(BaseControllerContext<DevelopersController> context,
            DevelopersRepository developersRepository) :
            base(context)
        {
            _developersRepository = developersRepository;
        }

        protected override BioRepository<Domain.Entities.Developer, int> GetRepository()
        {
            return _developersRepository;
        }

        protected override string GetUploadPath()
        {
            return "developers";
        }
    }
}