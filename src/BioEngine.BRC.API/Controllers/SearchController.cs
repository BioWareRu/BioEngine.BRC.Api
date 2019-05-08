using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Entities;
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

        private IBioRepository<T> GetRepository<T>() where T : class, IEntity
        {
            return HttpContext.RequestServices.GetRequiredService<IBioRepository<T>>();
        }

        private ISearchProvider<T> GetSearchProvider<T>() where T : BaseEntity
        {
            return HttpContext.RequestServices.GetRequiredService<ISearchProvider<T>>();
        }

        private async Task<bool> ReindexAsync<T>() where T : BaseEntity
        {
            var repository = GetRepository<T>();
            var searchProvider = GetSearchProvider<T>();

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
    }
}
