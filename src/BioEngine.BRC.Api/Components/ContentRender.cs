using System.Threading.Tasks;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Entities.Abstractions;
using BioEngine.BRC.Common.Web;
using BioEngine.BRC.Common.Web.RenderService;

namespace BioEngine.BRC.Api.Components
{
    public class ContentRender : IContentRender
    {
        private readonly IViewRenderService _renderService;

        public ContentRender(IViewRenderService renderService)
        {
            _renderService = renderService;
        }

        public async Task<string> RenderHtmlAsync(IContentEntity contentEntity, Site site,
            ContentEntityViewMode mode = ContentEntityViewMode.List)
        {
            return await _renderService.RenderToStringAsync("Content/Blocks",
                new ContentRendererModel(contentEntity, mode, site));
        }
    }

    public class ContentRendererModel
    {
        public ContentRendererModel(IContentEntity entity, ContentEntityViewMode mode, Site site)
        {
            Entity = entity;
            Mode = mode;
            Site = site;
        }

        public IContentEntity Entity { get; }
        public ContentEntityViewMode Mode { get; }
        public Site Site { get; }
    }
}
