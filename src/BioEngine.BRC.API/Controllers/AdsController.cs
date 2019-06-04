using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using BioEngine.Extra.Ads.Api;
using BioEngine.Extra.Ads.Entities;
using Microsoft.AspNetCore.Components;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("ads")]
    public class AdsController : AdsApiController
    {
        public AdsController(BaseControllerContext<Ad, ContentEntityQueryContext<Ad>, AdsRepository> context,
            BioEntityMetadataManager metadataManager, ContentBlocksRepository blocksRepository) : base(context,
            metadataManager, blocksRepository)
        {
        }
    }
}
