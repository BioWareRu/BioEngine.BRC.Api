using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.DB;
using BioEngine.Core.Entities;
using BioEngine.Core.Pages.Entities;
using BioEngine.Core.Posts.Entities;
using BioEngine.Core.Repository;
using BioEngine.Core.Search;
using BioEngine.Core.Web;
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

        private IBioRepository<T, TQueryContext> GetRepository<T, TQueryContext>() where T : class, IEntity
            where TQueryContext : QueryContext<T>
        {
            return HttpContext.RequestServices.GetRequiredService<IBioRepository<T, TQueryContext>>();
        }

        private ISearchProvider<T> GetSearchProvider<T>() where T : BaseEntity
        {
            return HttpContext.RequestServices.GetRequiredService<ISearchProvider<T>>();
        }

        private async Task<bool> ReindexAsync<T, TQueryContext>()
            where T : BaseEntity where TQueryContext : QueryContext<T>
        {
            var repository = GetRepository<T, TQueryContext>();
            var searchProvider = GetSearchProvider<T>();

            var entities = await repository.GetAllAsync();
            var result = await searchProvider.AddOrUpdateEntitiesAsync(entities.items);
            return result;
        }

        [HttpGet("reindex/games")]
        public Task<bool> ReindexGamesAsync()
        {
            return ReindexAsync<Game, ContentEntityQueryContext<Game>>();
        }

        [HttpGet("reindex/developers")]
        public Task<bool> ReindexDevelopersAsync()
        {
            return ReindexAsync<Developer, ContentEntityQueryContext<Developer>>();
        }

        [HttpGet("reindex/topics")]
        public Task<bool> ReindexTopicsAsync()
        {
            return ReindexAsync<Topic, ContentEntityQueryContext<Topic>>();
        }

        [HttpGet("reindex/posts")]
        public Task<bool> ReindexPostsAsync()
        {
            return ReindexAsync<Post, ContentEntityQueryContext<Post>>();
        }
        
        [HttpGet("reindex/pages")]
        public Task<bool> ReindexPagesAsync()
        {
            return ReindexAsync<Page, ContentEntityQueryContext<Page>>();
        }
    }
}
