using System;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Routing;
using BioEngine.BRC.Common.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("v1/[controller]")]
    public class StuffController : BaseController
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly PostsRepository _postsRepository;

        public StuffController(BaseControllerContext context,
            LinkGenerator linkGenerator,
            PostsRepository postsRepository) :
            base(context)
        {
            _linkGenerator = linkGenerator;
            _postsRepository = postsRepository;
        }

        [HttpGet("url.html")]
        public async Task<Uri> TestUrlAsync()
        {
            var posts = await _postsRepository.GetAllAsync();
            return _linkGenerator.GeneratePublicUrl(posts.items.First());
        }
    }
}
