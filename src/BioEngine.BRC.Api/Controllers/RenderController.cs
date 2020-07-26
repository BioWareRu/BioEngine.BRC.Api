using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.BRC.Common.Entities.Abstractions;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("v1/[controller]")]
    public class RenderController : BaseController
    {
        private readonly IContentRender _contentRender;
        private readonly ContentItemsRepository _contentItemsRepository;
        private readonly SitesRepository _sitesRepository;
        private readonly BrcApiOptions _options;

        public RenderController(BaseControllerContext context, IContentRender contentRender,
            ContentItemsRepository contentItemsRepository, IOptions<BrcApiOptions> options,
            SitesRepository sitesRepository) : base(context)
        {
            _contentRender = contentRender;
            _contentItemsRepository = contentItemsRepository;
            _sitesRepository = sitesRepository;
            _options = options.Value;
        }

        [HttpGet("post/{id}.html")]
        public async Task<IActionResult> PostAsync(Guid id)
        {
            var post = await _contentItemsRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var sites = await _sitesRepository.GetByIdsAsync(post.SiteIds);
            var mainSite = sites.FirstOrDefault(s => s.Id == _options.DefaultMainSiteId) ?? sites.First();

            var html = await _contentRender.RenderHtmlAsync(post, mainSite, ContentEntityViewMode.Entity);

            return Content(html, "text/html; charset=utf-8");
        }
    }
}
