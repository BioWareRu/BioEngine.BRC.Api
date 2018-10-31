using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class
        DevelopersController : SectionController<Developer, int, DeveloperData, Entities.Response.Developer, Entities.Request.DeveloperRequestItem>
    {
        public DevelopersController(BaseControllerContext<Developer, int> context) : base(context)
        {
        }

        protected override string GetUploadPath()
        {
            return "developers";
        }
    }
}