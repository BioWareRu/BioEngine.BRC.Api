using System;
using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web;
using BioEngine.BRC.Common.Web.Api;

namespace BioEngine.BRC.Api.Controllers
{
    public class
        DevelopersController : SectionController<Developer, DeveloperData, DevelopersRepository,
            Entities.Response.Developer,
            DeveloperRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "developers";
        }


        public DevelopersController(
            BaseControllerContext<Developer, Guid, DevelopersRepository> context,
            ContentBlocksRepository blocksRepository, StorageItemsRepository storageItemsRepository) : base(context,
            blocksRepository, storageItemsRepository)
        {
        }
    }
}
