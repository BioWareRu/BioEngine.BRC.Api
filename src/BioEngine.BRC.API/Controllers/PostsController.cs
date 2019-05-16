using System.Threading.Tasks;
using BioEngine.BRC.Api.Components;
using BioEngine.Core.API.Controllers;
using BioEngine.Core.API.Entities;
using BioEngine.Core.DB;
using BioEngine.Core.Repository;
using BioEngine.Core.Users;
using BioEngine.Core.Web;
using Post = BioEngine.Core.Entities.Post;

namespace BioEngine.BRC.Api.Controllers
{
    public class PostsController : ApiPostsController
    {
        private readonly BRCContentPublisher _publisher;

        public PostsController(BaseControllerContext<Post> context, BioEntityMetadataManager metadataManager,
            ContentBlocksRepository blocksRepository, IUserDataProvider userDataProvider,
            BRCContentPublisher publisher) : base(context,
            metadataManager, blocksRepository, userDataProvider)
        {
            _publisher = publisher;
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
    }
}
