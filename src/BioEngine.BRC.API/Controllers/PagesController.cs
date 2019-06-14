using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using BioEngine.Pages.Api;
using BioEngine.Pages.Db;
using BioEngine.Pages.Entities;

namespace BioEngine.BRC.Api.Controllers
{
    public class PagesController : ApiPagesController
    {
        public PagesController(BaseControllerContext<Page, PagesRepository> context, BioEntitiesManager entitiesManager,
            ContentBlocksRepository blocksRepository) : base(context, entitiesManager, blocksRepository)
        {
        }
    }
}
