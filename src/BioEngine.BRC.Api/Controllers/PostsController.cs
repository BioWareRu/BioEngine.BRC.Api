using System;
using System.Threading.Tasks;
using BioEngine.BRC.Api.Components;
using BioEngine.Core.Abstractions;
using BioEngine.Core.Api.Response;
using BioEngine.Core.DB;
using BioEngine.Core.Posts.Api;
using BioEngine.Core.Posts.Api.Entities;
using BioEngine.Core.Posts.Db;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using BioEngine.Extra.ContentTemplates.Db;
using BioEngine.Extra.ContentTemplates.Entities;
using BioEngine.Core.Posts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post = BioEngine.Core.Posts.Entities.Post;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ApiPostsController
    {
        private readonly BRCContentPublisher _publisher;
        private readonly ContentItemTemplatesRepository _templatesRepository;

        public PostsController(BaseControllerContext<Post, PostsRepository> context,
            BioEntitiesManager entitiesManager,
            ContentBlocksRepository blocksRepository, IUserDataProvider userDataProvider,
            BRCContentPublisher publisher, ContentItemTemplatesRepository templatesRepository) : base(context,
            entitiesManager, blocksRepository, userDataProvider)
        {
            _publisher = publisher;
            _templatesRepository = templatesRepository;
        }

        protected override async Task AfterSaveAsync(Post domainModel, PropertyChange[] changes = null,
            PostRequestItem request = null)
        {
            await base.AfterSaveAsync(domainModel, changes, request);
            await _publisher.PublishOrDeleteAsync(CurrentToken, domainModel, changes);
        }

        protected override async Task AfterDeleteAsync(Post domainModel)
        {
            await base.AfterDeleteAsync(domainModel);
            await _publisher.DeleteAsync(CurrentToken, domainModel);
        }

        [HttpGet("templates")]
        public async Task<ActionResult<ListResponse<ContentItemTemplate>>> GetTemplatesAsync()
        {
            var result = await _templatesRepository.GetTemplatesAsync<Post>();
            return Ok(new ListResponse<ContentItemTemplate>(result.items, result.itemsCount));
        }

        [HttpGet("new/template/{templateId}")]
        public async Task<ActionResult<Core.Posts.Api.Entities.Post>> CreateFromTemplateAsync(Guid templateId)
        {
            var content = await _templatesRepository.CreateFromTemplateAsync<Post, PostData>(templateId);
            if (content == null)
            {
                return NotFound();
            }

            return Ok(await MapRestModelAsync(content));
        }

        [HttpPost("templates/new/{postId}")]
        public async Task<ActionResult<ContentItemTemplate>> CreateTemplateAsync(Guid postId)
        {
            var post = await Repository.GetByIdAsync(postId);
            if (post == null)
            {
                return new ObjectResult(new RestResponse(StatusCodes.Status404NotFound,
                    new[] {new RestErrorResponse("Not Found")})) {StatusCode = StatusCodes.Status404NotFound};
            }

            var template = await _templatesRepository.CreateTemplateAsync<Post, PostData>(post);

            return Ok(template);
        }

        [HttpDelete("templates/{templateId}")]
        public async Task<ActionResult<ContentItemTemplate>> DeleteTemplatesAsync(Guid templateId)
        {
            var template = await _templatesRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                return new ObjectResult(new RestResponse(StatusCodes.Status404NotFound,
                    new[] {new RestErrorResponse("Not Found")})) {StatusCode = StatusCodes.Status404NotFound};
            }

            await _templatesRepository.DeleteAsync(template);

            return Ok(template);
        }
    }
}
