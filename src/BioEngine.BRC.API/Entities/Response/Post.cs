using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API.Models;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Post : ResponseContentEntityRestModel<Domain.Entities.Post, int, PostData>
    {
    }
}