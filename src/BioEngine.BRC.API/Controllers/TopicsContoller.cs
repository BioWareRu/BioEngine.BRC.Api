using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Repository;
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

        public TopicsController(BaseControllerContext<Topic> context, BioEntityMetadataManager metadataManager,
            ContentBlocksRepository blocksRepository) : base(context, metadataManager, blocksRepository)
        {
        }
    }
}
