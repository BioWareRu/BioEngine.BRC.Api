using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, int>
    {
        private readonly GamesRepository _gamesRepository;

        public GamesController(BaseControllerContext<GamesController> context, GamesRepository gamesRepository) : base(context)
        {
            _gamesRepository = gamesRepository;
        }

        protected override Game MapEntity(Game entity, Game newData)
        {
            entity = MapSectionData(entity, newData);
            entity.Data = newData.Data;
            return entity;
        }

        protected override BioRepository<Game, int> GetRepository()
        {
            return _gamesRepository;
        }

        protected override string GetUploadPath()
        {
            return "games";
        }
    }
}