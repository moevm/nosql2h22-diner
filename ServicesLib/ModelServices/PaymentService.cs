using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class PaymentService: BaseModelService<Payment>
{
    public PaymentService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }

    public async Task<Payment> CreateDefaultPayment(PaymentDto paymentDto)
    {
        var payment = new Payment
        {
            Status = paymentDto.Status, UserId = paymentDto.UserId, Type = paymentDto.Type, Price = paymentDto.Price,
            Description = paymentDto.Description,
        };
        
        await CreateAsync(payment);
        return payment;
    }
}