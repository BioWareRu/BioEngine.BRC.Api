using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Api.Models;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Request
{
    public class GameRequestItem : SectionRestModel<Game, GameData>
    {
        public GameRequestItem(LinkGenerator linkGenerator, SitesRepository sitesRepository,
            PropertiesProvider propertiesProvider) : base(linkGenerator, sitesRepository, propertiesProvider)
        {
        }
    }
}
