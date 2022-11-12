using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;
using Swashbuckle.Swagger.Annotations;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class PaymentController: Controller
{
    private PaymentService _paymentService;
    PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [Route("create-payments", Name = "createPayments")]
    public async Task<Payment> CreatePayment(PaymentDto paymentDto)
    {
        return await _paymentService.CreateDefaultPayment(paymentDto);
    }
    
    [HttpGet]
    [Route("get-payments", Name = "getPayments")]
    public async Task<List<Payment>> GetPayments()
    {
        return await _paymentService.FindAllAsync();
    }
    
    [HttpGet]
    [Route("get-payment", Name = "getPayment")]
    public async Task<Payment?> GetPayment(string id)
    {
        return await _paymentService.FindOneAsync(id);
    } 
}