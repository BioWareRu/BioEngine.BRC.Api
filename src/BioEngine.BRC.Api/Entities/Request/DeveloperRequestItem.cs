using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Properties;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Web.Api.Models;
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
