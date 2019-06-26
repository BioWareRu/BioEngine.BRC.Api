using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Abstractions;
using BioEngine.Core.Entities;
using BioEngine.Core.Posts.Entities;
using BioEngine.Core.Search;
using BioEngine.Core.Web;
using BioEngine.Core.Pages.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.BRC.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    public class SearchController : BaseController
    {
        public SearchController(BaseControllerContext context) : base(context)
        {
        }

        private IBioRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return HttpContext.RequestServices.GetRequiredService<IBioRepository<TEntity>>();
        }

        private ISearchProvider<TEntity> GetSearchProvider<TEntity>() where TEntity : BaseEntity
        {
            return HttpContext.RequestServices.GetRequiredService<ISearchProvider<TEntity>>();
        }

        private async Task<bool> ReindexAsync<TEntity>()
            where TEntity : BaseEntity
        {
            var repository = GetRepository<TEntity>();
            var searchProvider = GetSearchProvider<TEntity>();

            var entities = await repository.GetAllAsync();
            var result = await searchProvider.AddOrUpdateEntitiesAsync(entities.items);
            return result;
        }

        [HttpGet("reindex/games")]
        public Task<bool> ReindexGamesAsync()
        {
            return ReindexAsync<Game>();
        }

        [HttpGet("reindex/developers")]
        public Task<bool> ReindexDevelopersAsync()
        {
            return ReindexAsync<Developer>();
        }

        [HttpGet("reindex/topics")]
        public Task<bool> ReindexTopicsAsync()
        {
            return ReindexAsync<Topic>();
        }

        [HttpGet("reindex/posts")]
        public Task<bool> ReindexPostsAsync()
        {
            return ReindexAsync<Post>();
        }

        [HttpGet("reindex/pages")]
        public Task<bool> ReindexPagesAsync()
        {
            return ReindexAsync<Page>();
        }
    }
}
