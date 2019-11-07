using System;
using System.Threading.Tasks;
using BioEngine.BRC.Api.Components;
using BioEngine.Core.Api.Response;
using BioEngine.Core.Posts.Api;
using BioEngine.Core.Posts.Api.Entities;
using BioEngine.Core.Posts.Db;
using BioEngine.Core.Repository;
using BioEngine.Core.Web;
using BioEngine.Core.Posts.Entities;
using BioEngine.Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ApiPostsController<string>
    {
        private readonly BRCPostsPublisher _publisher;
        private readonly PostTemplatesRepository<string> _templatesRepository;

        public PostsController(BaseControllerContext<Core.Posts.Entities.Post<string>, PostsRepository<string>> context,
            ContentBlocksRepository blocksRepository, IUserDataProvider<string> userDataProvider,
            BRCPostsPublisher publisher, PostTemplatesRepository<string> templatesRepository,
            ICurrentUserProvider<string> currentUserProvider) : base(context,
            blocksRepository, userDataProvider, currentUserProvider)
        {
            _publisher = publisher;
            _templatesRepository = templatesRepository;
        }

        protected override async Task AfterSaveAsync(Core.Posts.Entities.Post<string> domainModel,
            PropertyChange[] changes = null,
            PostRequestItem<string> request = null)
        {
            await base.AfterSaveAsync(domainModel, changes, request);
            await _publisher.PublishOrDeleteAsync(domainModel, changes);
        }

        protected override async Task AfterDeleteAsync(Core.Posts.Entities.Post<string> domainModel)
        {
            await base.AfterDeleteAsync(domainModel);
            await _publisher.DeleteAsync(domainModel);
        }

        [HttpGet("templates")]
        public async Task<ActionResult<ListResponse<PostTemplate<string>>>> GetTemplatesAsync()
        {
            var result = await _templatesRepository.GetTemplatesAsync();
            return Ok(new ListResponse<PostTemplate<string>>(result.items, result.itemsCount));
        }

        [HttpGet("new/template/{templateId}")]
        public async Task<ActionResult<Core.Posts.Api.Entities.Post<string>>> CreateFromTemplateAsync(Guid templateId)
        {
            var content = await _templatesRepository.CreateFromTemplateAsync(templateId);
            if (content == null)
            {
                return NotFound();
            }

            return Ok(await MapRestModelAsync(content));
        }

        [HttpPost("templates/new/{postId}")]
        public async Task<ActionResult<PostTemplate<string>>> CreateTemplateAsync(Guid postId)
        {
            var post = await Repository.GetByIdAsync(postId);
            if (post == null)
            {
                return new ObjectResult(new RestResponse(StatusCodes.Status404NotFound,
                    new[] {new RestErrorResponse("Not Found")})) {StatusCode = StatusCodes.Status404NotFound};
            }

            var template = await _templatesRepository.CreateTemplateAsync(post);

            return Ok(template);
        }

        [HttpDelete("templates/{templateId}")]
        public async Task<ActionResult<PostTemplate<string>>> DeleteTemplatesAsync(Guid templateId)
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
