using System;
using BioEngine.BRC.Common.Ads.Api;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web;
using Microsoft.AspNetCore.Components;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("ads")]
    public class AdsController : AdsApiController
    {
        public AdsController(BaseControllerContext<Ad, Guid, AdsRepository> context,
            ContentBlocksRepository blocksRepository) : base(context,
            blocksRepository)
        {
        }
    }
}
