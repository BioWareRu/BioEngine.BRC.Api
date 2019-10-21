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
        public AdsController(BaseControllerContext<Ad, AdsRepository> context,
            ContentBlocksRepository blocksRepository) : base(context,
            blocksRepository)
        {
        }
    }
}
