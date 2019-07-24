using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Api.Models;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Request
{
    public class DeveloperRequestItem : SectionRestModel<Developer, DeveloperData>
    {
        public DeveloperRequestItem(LinkGenerator linkGenerator, SitesRepository sitesRepository,
            PropertiesProvider propertiesProvider) : base(linkGenerator, sitesRepository, propertiesProvider)
        {
        }
    }
}
