using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API.Models;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Game : ResponseSectionRestModel<Domain.Entities.Game, GameData>
    {
        public Game(LinkGenerator linkGenerator, SitesRepository sitesRepository) : base(linkGenerator, sitesRepository)
        {
        }
    }
}
