using BioEngine.Core.API;
using BioEngine.Core.API.Entities;
using BioEngine.Core.Entities;
using BioEngine.Core.Web;

namespace BioEngine.BRC.Api.Controllers
{
    public class ContentController : ContentController<ContentEntityItem, ContentItem, int>
    {
        public ContentController(BaseControllerContext<ContentItem, int> context) : base(context)
        {
        }
    }
}