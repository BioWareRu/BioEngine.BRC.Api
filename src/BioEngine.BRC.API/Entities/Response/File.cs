using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API.Models;

namespace BioEngine.BRC.Api.Entities.Response
{
    public class File : ResponseContentEntityRestModel<Domain.Entities.File, int, FileData>
    {
    }
}