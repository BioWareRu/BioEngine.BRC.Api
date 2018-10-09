using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using Topic = BioEngine.BRC.Api.Entities.Topic;

namespace BioEngine.BRC.Api.Controllers
{
    public class TopicsController : SectionController<Topic, Domain.Entities.Topic, int, TopicData>
    {
        private readonly TopicsRepository _topicsRepository;

        public TopicsController(BaseControllerContext<TopicsController> context, TopicsRepository topicsRepository) :
            base(context)
        {
            _topicsRepository = topicsRepository;
        }

        protected override BioRepository<Domain.Entities.Topic, int> GetRepository()
        {
            return _topicsRepository;
        }

        protected override string GetUploadPath()
        {
            return "topics";
        }
    }
}