using System.Collections.Generic;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class
        DevelopersController : SectionController<Developer, DeveloperData, Entities.Response.Developer,
            Entities.Request.DeveloperRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "developers";
        }

        public DevelopersController(BaseControllerContext<Developer> context,
            IEnumerable<EntityMetadata> entityMetadataList, BioContext dbContext) : base(context, entityMetadataList,
            dbContext)
        {
        }
    }
}
