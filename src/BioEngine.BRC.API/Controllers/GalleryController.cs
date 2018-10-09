using System;
using System.IO;
using System.Threading.Tasks;
using BioEngine.BRC.Api.Entities;
using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using BioEngine.Core.Storage;
using Microsoft.AspNetCore.Mvc;
using Gallery = BioEngine.BRC.Api.Entities.Gallery;

namespace BioEngine.BRC.Api.Controllers
{
    public class GalleryController : ContentController<Gallery, Domain.Entities.Gallery, int, GalleryData>
    {
        private readonly GalleryRepository _galleryRepository;

        public GalleryController(BaseControllerContext<GalleryController> context, GalleryRepository galleryRepository)
            : base(context)
        {
            _galleryRepository = galleryRepository;
        }

        protected override BioRepository<Domain.Entities.Gallery, int> GetRepository()
        {
            return _galleryRepository;
        }

        public override async Task<ActionResult<StorageItem>> Upload(string name)
        {
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                return await Storage.SaveFile(ms.GetBuffer(), name,
                    $"gallery/{DateTimeOffset.UtcNow:yyyyddMM}");
            }
        }
    }
}