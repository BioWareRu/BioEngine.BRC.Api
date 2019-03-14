using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, GameData, Entities.Response.Game, Entities.Request.GameRequestItem>
    {
        public GamesController(BaseControllerContext<Game> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            return "games";
        }
    }
}
