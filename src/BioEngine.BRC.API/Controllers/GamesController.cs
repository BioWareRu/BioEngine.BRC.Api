using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Game = BioEngine.BRC.Api.Entities.Game;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, Domain.Entities.Game, int, GameData>
    {
        public GamesController(BaseControllerContext<Domain.Entities.Game, int> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            return "games";
        }
    }
}