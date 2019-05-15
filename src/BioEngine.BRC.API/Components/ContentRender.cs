using System.Threading.Tasks;
using BioEngine.Core.Entities;
using BioEngine.Core.Web;
using BioEngine.Core.Web.RenderService;

namespace BioEngine.BRC.Api.Components
{
    public class ContentRender : IContentRender
    {
        private readonly IViewRenderService _renderService;

        public ContentRender(IViewRenderService renderService)
        {
            _renderService = renderService;
        }

        public async Task<string> RenderHtmlAsync(IContentEntity contentEntity,
            ContentEntityViewMode mode = ContentEntityViewMode.List)
        {
            return await _renderService.RenderToStringAsync("Content/Blocks",
                new ContentRendererModel(contentEntity, mode));
        }
    }

    public class ContentRendererModel
    {
        public ContentRendererModel(IContentEntity entity, ContentEntityViewMode mode)
        {
            Entity = entity;
            Mode = mode;
        }

        public IContentEntity Entity { get; }
        public ContentEntityViewMode Mode { get; }
    }
}
