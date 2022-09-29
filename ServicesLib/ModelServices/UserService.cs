using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class UserService: BaseModelService<User, UserDbConfig>
{
    public UserService(IOptions<UserDbConfig> dbConfig) : base(dbConfig)
    { }
}