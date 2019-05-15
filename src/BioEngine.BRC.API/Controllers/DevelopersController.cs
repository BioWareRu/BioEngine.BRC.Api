using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class
        DevelopersController : SectionController<Developer, DeveloperData, Entities.Response.Developer,
            DeveloperRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "developers";
        }


        public DevelopersController(BaseControllerContext<Developer> context, BioEntityMetadataManager metadataManager,
            ContentBlocksRepository blocksRepository) : base(context, metadataManager, blocksRepository)
        {
        }
    }
}
