using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API.Models;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Developer : ResponseSectionRestModel<Domain.Entities.Developer, DeveloperData>
    {
        public Developer(LinkGenerator linkGenerator, SitesRepository sitesRepository) : base(linkGenerator, sitesRepository)
        {
        }
    }
}
