using System;
using System.IO;
using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.Storage;
using BioEngine.Core.Web;
using Microsoft.AspNetCore.Mvc;
using File = BioEngine.BRC.Domain.Entities.File;

namespace BioEngine.BRC.Api.Controllers
{
    public class FilesController : ContentController<Entities.File, File, int, FileData>
    {
        public FilesController(BaseControllerContext<File, int> context) : base(context)
        {
        }

        public override async Task<ActionResult<StorageItem>> UploadAsync(string name)
        {
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);
                return await Storage.SaveFileAsync(ms.GetBuffer(), name,
                    $"files/{DateTimeOffset.UtcNow:yyyyddMM}");
            }
        }
    }
}