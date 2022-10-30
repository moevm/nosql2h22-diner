using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class PaymentService: BaseModelService<Payment, PaymentDbConfig>
{
    public PaymentService(IOptions<PaymentDbConfig> dbConfig) : base(dbConfig)
    { }
}