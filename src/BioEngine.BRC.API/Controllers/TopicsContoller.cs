using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Topic = BioEngine.BRC.Api.Entities.Topic;

namespace BioEngine.BRC.Api.Controllers
{
    public class TopicsController : SectionController<Topic, Domain.Entities.Topic, int, TopicData>
    {
        public TopicsController(BaseControllerContext<Domain.Entities.Topic, int> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            return "topics";
        }
    }
}