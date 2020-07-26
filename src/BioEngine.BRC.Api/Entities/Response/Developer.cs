using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Properties;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web.Api.Models;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Developer : ResponseSectionRestModel<BioEngine.BRC.Common.Entities.Developer, DeveloperData>
    {
        public Developer(LinkGenerator linkGenerator, SitesRepository sitesRepository,
            PropertiesProvider propertiesProvider) : base(linkGenerator, sitesRepository, propertiesProvider)
        {
        }
    }
}
