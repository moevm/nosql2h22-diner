using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class PaymentService: BaseModelService<Payment>
{
    public PaymentService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }
}