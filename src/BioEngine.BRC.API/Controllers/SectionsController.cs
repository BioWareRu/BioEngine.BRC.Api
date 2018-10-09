using System;
using System.Threading.Tasks;
using BioEngine.BRC.Api.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Entities;
using BioEngine.Core.Interfaces;
using BioEngine.Core.Repository;
using Section = BioEngine.Core.API.Entities.Section;

namespace BioEngine.BRC.Api.Controllers
{
    public class SectionsController : SectionController<Section, Core.Entities.Section, int>
    {
        private readonly SectionsRepository _sectionsRepository;

        public SectionsController(BaseControllerContext<SectionsController> context,
            SectionsRepository sectionsRepository) : base(context)
        {
            _sectionsRepository = sectionsRepository;
        }

        protected override BioRepository<Core.Entities.Section, int> GetRepository()
        {
            return _sectionsRepository;
        }

        protected override string GetUploadPath()
        {
            throw new NotImplementedException();
        }
    }
}