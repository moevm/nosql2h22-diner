using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class CommentService: BaseModelService<Comment>
{
    public CommentService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }
}