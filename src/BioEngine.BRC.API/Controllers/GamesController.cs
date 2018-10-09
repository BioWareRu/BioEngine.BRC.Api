using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Game = BioEngine.BRC.Api.Entities.Game;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, Domain.Entities.Game, int, GameData>
    {
        private readonly GamesRepository _gamesRepository;

        public GamesController(BaseControllerContext<GamesController> context, GamesRepository gamesRepository) :
            base(context)
        {
            _gamesRepository = gamesRepository;
        }

        protected override BioRepository<Domain.Entities.Game, int> GetRepository()
        {
            return _gamesRepository;
        }

        protected override string GetUploadPath()
        {
            return "games";
        }
    }
}