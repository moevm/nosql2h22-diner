using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ResourceService: BaseModelService<Resource>
{
    public ResourceService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }
}