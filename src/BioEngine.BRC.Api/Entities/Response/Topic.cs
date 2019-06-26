using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Api.Models;
using BioEngine.Core.Repository;
using Microsoft.AspNetCore.Routing;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Topic : ResponseSectionRestModel<Domain.Entities.Topic, TopicData>
    {
        public Topic(LinkGenerator linkGenerator, SitesRepository sitesRepository) : base(linkGenerator, sitesRepository)
        {
        }
    }
}
