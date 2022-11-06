using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class UserService: BaseModelService<User>
{
    public UserService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }
}