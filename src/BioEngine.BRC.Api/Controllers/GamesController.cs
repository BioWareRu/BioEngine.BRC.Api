using System;
using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web;
using BioEngine.BRC.Common.Web.Api;

namespace BioEngine.BRC.Api.Controllers
{
    public class GamesController : SectionController<Game, GameData, GamesRepository, Entities.Response.Game,
        GameRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "games";
        }


        public GamesController(BaseControllerContext<Game, Guid, GamesRepository> context,
            ContentBlocksRepository blocksRepository, StorageItemsRepository storageItemsRepository) : base(context,
            blocksRepository, storageItemsRepository)
        {
        }
    }
}
