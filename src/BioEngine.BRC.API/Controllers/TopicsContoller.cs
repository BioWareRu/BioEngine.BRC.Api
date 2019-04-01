using System.Collections.Generic;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class TopicsController : SectionController<Topic, TopicData, Entities.Response.Topic,
        Entities.Request.TopicRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "topics";
        }

        public TopicsController(BaseControllerContext<Topic> context, IEnumerable<EntityMetadata> entityMetadataList,
            BioContext dbContext) : base(context, entityMetadataList, dbContext)
        {
        }
    }
}
