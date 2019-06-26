using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.Api;
using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class
        DevelopersController : SectionController<Developer, DeveloperData, DevelopersRepository,
            Entities.Response.Developer,
            DeveloperRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "developers";
        }


        public DevelopersController(
            BaseControllerContext<Developer, DevelopersRepository> context,
            BioEntitiesManager entitiesManager, ContentBlocksRepository blocksRepository) : base(context,
            entitiesManager, blocksRepository)
        {
        }
    }
}
