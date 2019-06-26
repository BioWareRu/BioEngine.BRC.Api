using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API.Models;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Request
{
    public class DeveloperRequestItem : SectionRestModel<Developer, DeveloperData>
    {
        public DeveloperRequestItem(LinkGenerator linkGenerator, SitesRepository sitesRepository) : base(linkGenerator, sitesRepository)
        {
        }
    }
}
