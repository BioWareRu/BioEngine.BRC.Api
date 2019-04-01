using System.Collections.Generic;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, GameData, Entities.Response.Game, Entities.Request.GameRequestItem>
    {

        protected override string GetUploadPath()
        {
            return "games";
        }

        public GamesController(BaseControllerContext<Game> context, IEnumerable<EntityMetadata> entityMetadataList, BioContext dbContext) : base(context, entityMetadataList, dbContext)
        {
        }
    }
}
