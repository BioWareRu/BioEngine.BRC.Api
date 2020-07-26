using System;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Pages.Api;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class PagesController : ApiPagesController
    {
        public PagesController(BaseControllerContext<Page, Guid, PagesRepository> context,
            ContentBlocksRepository blocksRepository) : base(context, blocksRepository)
        {
        }
    }
}
