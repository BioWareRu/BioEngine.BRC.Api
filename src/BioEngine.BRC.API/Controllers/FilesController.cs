using BioEngine.BRC.Domain.Entities;
using BioEngine.BRC.Domain.Repository;
using BioEngine.Core.API;
using BioEngine.Core.Repository;

namespace BioEngine.BRC.Api.Controllers
{
    public class FilesController : ContentController<File, int>
    {
        private readonly FilesRepository _filesRepository;

        public FilesController(BaseControllerContext<FilesController> context, FilesRepository filesRepository) : base(context)
        {
            _filesRepository = filesRepository;
        }

        protected override File MapEntity(File entity, File newData)
        {
            entity = MapContentData(entity, newData);
            entity.Data = newData.Data;
            return entity;
        }

        protected override BioRepository<File, int> GetRepository()
        {
            return _filesRepository;
        }
    }
}