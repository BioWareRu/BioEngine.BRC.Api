using System;
using BioEngine.Core.API;
using BioEngine.Core.Web;
using Section = BioEngine.Core.API.Entities.Section;

namespace BioEngine.BRC.Api.Controllers
{
    public class SectionsController : SectionController<Section, Core.Entities.Section, int>
    {
        public SectionsController(BaseControllerContext<Core.Entities.Section, int> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            throw new NotImplementedException();
        }
    }
}