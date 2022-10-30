using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class CommentService: BaseModelService<Comment, CommentDbConfig>
{
    public CommentService(IOptions<CommentDbConfig> dbConfig) : base(dbConfig)
    {
    }
}