using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Routing;
using BioEngine.BRC.Common.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace BioEngine.BRC.Api.Controllers
{
    [Route("v1/[controller]")]
    public class StuffController : BaseController
    {
        private readonly StorageItemsRepository _storageItemsRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly PostsRepository _postsRepository;
        private readonly ILogger<StuffController> _logger;

        public StuffController(BaseControllerContext context, StorageItemsRepository storageItemsRepository,
            LinkGenerator linkGenerator,
            PostsRepository postsRepository,
            ILogger<StuffController> logger) :
            base(context)
        {
            _storageItemsRepository = storageItemsRepository;
            _linkGenerator = linkGenerator;
            _postsRepository = postsRepository;
            _logger = logger;
        }

        [HttpGet("hash.html")]
        public async Task<IActionResult> HashAsync()
        {
            var httpClient = new HttpClient();
            var sha256 = SHA256.Create();
            await _storageItemsRepository.BeginBatchAsync();
            var items = await _storageItemsRepository.GetAllAsync();
            foreach (var item in items.items)
            {
                try
                {
                    var file = await httpClient.GetByteArrayAsync(item.PublicUri);
                    var hash = HashBytesToString(sha256.ComputeHash(file));
                    item.Hash = hash;
                    await _storageItemsRepository.UpdateAsync(item);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.ToString());
                }
            }

            await _storageItemsRepository.CommitBatchAsync();

            return Ok();
        }

        private static string HashBytesToString(byte[] bytes)
        {
            return bytes.Aggregate("", (current, b) => current + b.ToString("x2"));
        }

        [HttpGet("url.html")]
        public async Task<Uri> TestUrlAsync()
        {
            var posts = await _postsRepository.GetAllAsync();
            return _linkGenerator.GeneratePublicUrl(posts.items.First());
        }

        [HttpGet("files.html")]
        public async Task<List<string>> ListFilesAsync()
        {
            var files = await _storageItemsRepository.GetAllAsync();
            var list = new List<string>();
            foreach (var file in files.items)
            {
                list.Add(file.FilePath);
                if (file.PictureInfo != null)
                {
                    list.Add(file.PictureInfo.SmallThumbnail.FilePath);
                    list.Add(file.PictureInfo.MediumThumbnail.FilePath);
                }
            }

            return list;
        }
    }
}
