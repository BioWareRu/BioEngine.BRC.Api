using System;
using System.Threading.Tasks;
using BioEngine.Core.API;
using BioEngine.Core.Entities;
using BioEngine.Core.Repository;
using BioEngine.Core.Storage;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.BRC.Api.Controllers
{
    public class SectionsController : SectionController<Section, int>
    {
        private readonly SectionsRepository _sectionsRepository;

        public SectionsController(BaseControllerContext<SectionsController> context,
            SectionsRepository sectionsRepository) : base(context)
        {
            _sectionsRepository = sectionsRepository;
        }

        protected override Section MapEntity(Section entity, Section newData)
        {
            return entity;
        }

        protected override BioRepository<Section, int> GetRepository()
        {
            return _sectionsRepository;
        }

        protected override string GetUploadPath()
        {
            throw new NotImplementedException();
        }
    }
}