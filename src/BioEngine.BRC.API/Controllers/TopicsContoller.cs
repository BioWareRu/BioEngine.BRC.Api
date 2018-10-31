using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class TopicsController : SectionController<Topic, int, TopicData, Entities.Response.Topic,
        Entities.Request.TopicRequestItem>
    {
        public TopicsController(BaseControllerContext<Topic, int> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            return "topics";
        }
    }
}