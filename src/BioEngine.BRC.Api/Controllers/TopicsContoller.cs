using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.Api;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class TopicsController : SectionController<Topic, TopicData, TopicsRepository, Entities.Response.Topic,
        TopicRequestItem>
    {
        protected override string GetUploadPath()
        {
            return "topics";
        }


        public TopicsController(
            BaseControllerContext<Topic, TopicsRepository> context,
            ContentBlocksRepository blocksRepository) : base(context,
            blocksRepository)
        {
        }
    }
}
