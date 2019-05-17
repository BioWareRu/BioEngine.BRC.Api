using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Core.Entities;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("v1/[controller]")]
    public class RenderController : BaseController
    {
        private readonly IContentRender _contentRender;
        private readonly PostsRepository _postsRepository;
        private readonly SitesRepository _sitesRepository;
        private readonly BrcApiOptions _options;

        public RenderController(BaseControllerContext context, IContentRender contentRender,
            PostsRepository postsRepository, IOptions<BrcApiOptions> options,
            SitesRepository sitesRepository) : base(context)
        {
            _contentRender = contentRender;
            _postsRepository = postsRepository;
            _sitesRepository = sitesRepository;
            _options = options.Value;
        }

        [HttpGet("post/{id}.html")]
        public async Task<IActionResult> PostAsync(Guid id)
        {
            var post = await _postsRepository.GetByIdAsync(id);
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
