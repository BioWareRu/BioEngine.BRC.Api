using System;
using System.IO;
using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;
using BioEngine.Core.Storage;
using Microsoft.AspNetCore.Mvc;
using File = BioEngine.BRC.Domain.Entities.File;

namespace BioEngine.BRC.Api.Controllers
{
    public class FilesController : ContentController<Entities.File, File, int, FileData>
    {
        private readonly FilesRepository _filesRepository;

        public FilesController(BaseControllerContext<FilesController> context, FilesRepository filesRepository) :
            base(context)
        {
            _filesRepository = filesRepository;
        }

        protected override BioRepository<File, int> GetRepository()
        {
            return _filesRepository;
        }

        public override async Task<ActionResult<StorageItem>> Upload(string name)
        {
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                return await Storage.SaveFile(ms.GetBuffer(), name,
                    $"files/{DateTimeOffset.UtcNow:yyyyddMM}");
            }
        }
    }
}