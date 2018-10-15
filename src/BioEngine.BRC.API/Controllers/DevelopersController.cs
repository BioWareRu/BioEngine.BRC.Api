using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Developer = BioEngine.BRC.Api.Entities.Developer;

namespace BioEngine.BRC.Api.Controllers
{
    public class DevelopersController : SectionController<Developer, Domain.Entities.Developer, int, DeveloperData>
    {
        public DevelopersController(BaseControllerContext<Domain.Entities.Developer, int> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            return "developers";
        }
    }
}