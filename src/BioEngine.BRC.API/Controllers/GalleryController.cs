using System;
using System.IO;
using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using BioEngine.Core.Storage;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    public class GalleryController : ContentController<Gallery, int>
    {
        private readonly GalleryRepository _galleryRepository;

        public GalleryController(BaseControllerContext<GalleryController> context, GalleryRepository galleryRepository) : base(context)
        {
            _galleryRepository = galleryRepository;
        }

        protected override Gallery MapEntity(Gallery entity, Gallery newData)
        {
            entity = MapContentData(entity, newData);
            entity.Data = newData.Data;
            return entity;
        }

        protected override BioRepository<Gallery, int> GetRepository()
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