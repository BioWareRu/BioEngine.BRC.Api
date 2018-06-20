using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class TopicsContoller : SectionController<Topic, int>
    {
        private readonly TopicsRepository _topicsRepository;

        public TopicsContoller(BaseControllerContext<TopicsContoller> context, TopicsRepository topicsRepository) :
            base(context)
        {
            _topicsRepository = topicsRepository;
        }

        protected override BioRepository<Topic, int> GetRepository()
        {
            return _topicsRepository;
        }

        protected override Topic MapEntity(Topic entity, Topic newData)
        {
            entity = MapSectionData(entity, newData);
            entity.Data = newData.Data;
            return entity;
        }
    }
}