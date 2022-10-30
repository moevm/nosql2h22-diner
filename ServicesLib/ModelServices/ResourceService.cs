using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ResourceService: BaseModelService<Resource, ResourceDbConfig>
{
    public ResourceService(IOptions<ResourceDbConfig> dbConfig) : base(dbConfig)
    {
    }
}