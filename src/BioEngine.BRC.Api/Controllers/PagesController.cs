using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using BioEngine.Core.Pages.Api;
using BioEngine.Core.Pages.Db;
using BioEngine.Core.Pages.Entities;

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
