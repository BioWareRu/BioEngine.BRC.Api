using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Api.Models;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Game : ResponseSectionRestModel<Domain.Entities.Game, GameData>
    {
        public Game(LinkGenerator linkGenerator, SitesRepository sitesRepository, PropertiesProvider propertiesProvider)
            : base(linkGenerator, sitesRepository, propertiesProvider)
        {
        }
    }
}
