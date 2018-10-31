using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API.Models;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class Gallery : ResponseContentEntityRestModel<Domain.Entities.Gallery, int, GalleryData>
    {
    }
}