using System;
using System.IO;
using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Storage;
using BioEngine.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Gallery = BioEngine.BRC.Api.Entities.Gallery;

namespace BioEngine.BRC.Api.Controllers
{
    public class GalleryController : ContentController<Gallery, Domain.Entities.Gallery, int, GalleryData>
    {
        public GalleryController(BaseControllerContext<Domain.Entities.Gallery, int> context) : base(context)
        {
        }

        public override async Task<ActionResult<StorageItem>> UploadAsync(string name)
        {
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                return await Storage.SaveFileAsync(ms.GetBuffer(), name,
                    $"gallery/{DateTimeOffset.UtcNow:yyyyddMM}");
            }
        }
    }
}