using System;
using System.Threading.Tasks;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sitko.Core.Repository;
using Sitko.Core.Search;

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

        private IRepository<TEntity, TEntityPk> GetRepository<TEntity, TEntityPk>()
            where TEntity : class, IEntity<TEntityPk>
        {
            return HttpContext.RequestServices.GetRequiredService<IRepository<TEntity, TEntityPk>>();
        }

        private ISearchProvider<TEntity, Guid> GetSearchProvider<TEntity>() where TEntity : BaseEntity
        {
            return HttpContext.RequestServices.GetRequiredService<ISearchProvider<TEntity, Guid>>();
        }

        private async Task<bool> ReindexAsync<TEntity>()
            where TEntity : BaseEntity
        {
            var repository = GetRepository<TEntity, Guid>();
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
