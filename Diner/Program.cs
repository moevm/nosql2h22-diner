using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicesLib;
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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(500);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/auth";
    });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => {
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diner API");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();