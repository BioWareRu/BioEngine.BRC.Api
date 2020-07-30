using System;
using System.Threading.Tasks;
using BioEngine.BRC.Api.Components;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Posts.Api;
using BioEngine.BRC.Common.Posts.Api.Entities;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Users;
using BioEngine.BRC.Common.Web;
using BioEngine.BRC.Common.Web.Api.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sitko.Core.Repository;
using Post = BioEngine.BRC.Common.Entities.Post;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ApiPostsController
    {
        private readonly BRCPostsPublisher _publisher;
        private readonly PostTemplatesRepository<string> _templatesRepository;


        public PostsController(BaseControllerContext<Post, Guid, PostsRepository> context,
            ContentBlocksRepository blocksRepository, IUserDataProvider userDataProvider,
            ICurrentUserProvider currentUserProvider, BRCPostsPublisher publisher,
            PostTemplatesRepository<string> templatesRepository, StorageItemsRepository storageItemsRepository) : base(
            context, blocksRepository, userDataProvider,
            currentUserProvider, storageItemsRepository)
        {
            _publisher = publisher;
            _templatesRepository = templatesRepository;
        }

        protected override async Task AfterSaveAsync(Post domainModel,
            PropertyChange[] changes = null,
            PostRequestItem request = null)
        {
            await base.AfterSaveAsync(domainModel, changes, request);
            await _publisher.PublishOrDeleteAsync(domainModel, changes);
        }

        protected override async Task AfterDeleteAsync(Post domainModel)
        {
            await base.AfterDeleteAsync(domainModel);
            await _publisher.DeleteAsync(domainModel);
        }

        [HttpGet("templates")]
        public async Task<ActionResult<ListResponse<PostTemplate>>> GetTemplatesAsync()
        {
            var result = await _templatesRepository.GetTemplatesAsync();
            return Ok(new ListResponse<PostTemplate>(result.items, result.itemsCount));
        }

        [HttpGet("new/template/{templateId}")]
        public async Task<ActionResult<BioEngine.BRC.Common.Posts.Api.Entities.Post>> CreateFromTemplateAsync(
            Guid templateId)
        {
            var content = await _templatesRepository.CreateFromTemplateAsync(templateId);

            return Ok(await MapRestModelAsync(content));
        }

        [HttpPost("templates/new/{postId}")]
        public async Task<ActionResult<PostTemplate>> CreateTemplateAsync(Guid postId)
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
        public async Task<ActionResult<PostTemplate>> DeleteTemplatesAsync(Guid templateId)
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
