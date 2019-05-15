using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, GameData, Entities.Response.Game,
        GameRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "games";
        }


        public GamesController(BaseControllerContext<Game> context, BioEntityMetadataManager metadataManager,
            ContentBlocksRepository blocksRepository) : base(context, metadataManager, blocksRepository)
        {
        }
    }
}
