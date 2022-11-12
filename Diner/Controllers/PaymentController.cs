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

    [HttpPost("create-payment")]
    [SwaggerOperation("create-payment")]
    public async Task<Payment> CreatePayment(PaymentDto paymentDto)
    {
        return await _paymentService.CreateDefaultPayment(paymentDto);
    }
    
    // [HttpPost("get-payments")]
    // public async Task<List<Payment>> GetPayments()
    // {
    // }
    
    // [HttpPost("get-payment")]
    // public async Task<List<Payment>> GetPayment()
    // {
    // } 
}