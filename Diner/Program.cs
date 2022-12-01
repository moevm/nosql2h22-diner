using System.Text.Json;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using ServicesLib;
using ServicesLib.ModelServices;
using UtilsLib.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("DbConfig"));
builder.Services.AddServicesLib(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Diner API", Version = "v1" });
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(500);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/auth";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Clear();
            context.Response.StatusCode = 401;
            return Task.FromResult(0);
        };
    });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => {
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diner API");
});
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler(exceptionHandlerApp => {
    exceptionHandlerApp.Run(async context => {
        Console.WriteLine(context);
        Console.WriteLine(JsonSerializer.Serialize(context));
    });
});

var userService = app.Services.GetService<UserService>();
var dishService = app.Services.GetService<DishService>();
var paymentService = app.Services.GetService<PaymentService>();
var resourceService = app.Services.GetService<ResourceService>();

var users = await userService.FindAllAsync();
var dishes = await dishService.FindAllAsync();
var payments = await paymentService.FindAllAsync();
var resources = await resourceService.FindAllAsync();
User user = new User();
if (users.Count == 0)
{
    user = await userService.CreateUserWithDefaults(new CreateUserDto() { FullName = "Test Test", Login = "test", Role = UserRole.Admin, Password = "test-test"});
}

if (dishes.Count == 0)
{
    await dishService.CreateDish(new DishDto()
    {
        Name = "Onion Soup",
        Description = "Soup from onions",
        DishType = DishType.Soup,
        Price = 15,
    });
}

if (payments.Count == 0)
{
    await paymentService.CreateDefaultPayment(new PaymentDto()
    {
        Description = "VERY GOOD",
        Price = 100,
        Number = 0,
        Status = PaymentStatus.Approval,
        Type = PaymentType.Lease,
        UserId = user.Id,
    });
    await paymentService.CreateDefaultPayment(new PaymentDto()
    {
        Description = "SUPER",
        Price = 200,
        Number = 0,
        Status = PaymentStatus.Approval,
        Type = PaymentType.Lease,
        UserId = user.Id,
    });
    await paymentService.CreateDefaultPayment(new PaymentDto()
    {
        Description = "VERY FINE",
        Price = 500,
        Number = 0,
        Status = PaymentStatus.Approval,
        Type = PaymentType.Lease,
        UserId = user.Id,
    });
    
}

if (resources.Count == 0)
{
    await resourceService.CreateResource(new ResourceDto()
    {
        Amount = 1000,
        Name = "Onion",
        Unit = Unit.Kg,
    });
}


app.Run();