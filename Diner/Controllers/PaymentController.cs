using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServicesLib.ModelServices;
using Swashbuckle.Swagger.Annotations;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class PaymentController: Controller
{
    private PaymentService _paymentService;
    public PaymentController(PaymentService paymentService)
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
    public async Task<List<Payment>> GetPayments(int? number, int? gt, int? lt)
    {
        if (number == null && gt == null && lt == null) return await _paymentService.FindAllAsync();
        var filterNumber = number != null
            ? Builders<Payment>.Filter.Where(x => x.Number.Equals(number))
            : Builders<Payment>.Filter.Where(x => true);
        var gtLtFilter = Builders<Payment>.Filter.Where(x => x.Price >= gt && x.Price <= lt);
        if (gt == null && lt == null) gtLtFilter = Builders<Payment>.Filter.Where(x => true);
        return await _paymentService.WhereManyAsync(
            Builders<Payment>.Filter.Where(x => filterNumber.Inject() && gtLtFilter.Inject()));
    }
    
    [HttpGet]
    [Route("get-payment", Name = "getPayment")]
    [ProducesResponseType(typeof(Payment), 200)]
    public async Task<IActionResult> GetPayment(string id)
    {
        var payment = await _paymentService.FindOneAsync(id);
        HttpContext.Response.Headers.Add("Content-Type", "application/json");
        return payment != null ? Ok(payment) : Json(null);
    } 
}