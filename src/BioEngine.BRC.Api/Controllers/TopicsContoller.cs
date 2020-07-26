using System;
using BioEngine.BRC.Api.Entities.Request;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web;
using BioEngine.BRC.Common.Web.Api;

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
            BaseControllerContext<Topic, Guid, TopicsRepository> context,
            ContentBlocksRepository blocksRepository) : base(context,
            blocksRepository)
        {
        }
    }
}
