using System;
using System.Threading.Tasks;
using BioEngine.Core.Entities;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("v1/[controller]")]
    public class RenderController : BaseController
    {
        private readonly IContentRender _contentRender;
        private readonly PostsRepository _postsRepository;

        public RenderController(BaseControllerContext context, IContentRender contentRender,
            PostsRepository postsRepository) : base(context)
        {
            _contentRender = contentRender;
            _postsRepository = postsRepository;
        }

        [HttpGet("post/{id}.html")]
        public async Task<IActionResult> PostAsync(Guid id)
        {
            var post = await _postsRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var html = await _contentRender.RenderHtmlAsync(post, ContentEntityViewMode.Entity);

            return Content(html, "text/html; charset=utf-8");
        }
    }
}
