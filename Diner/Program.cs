using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServicesLib;
using UtilsLib.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "theBoris",
        ValidAudience = "theBoris",
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes("thereIsCoolKeyFromConfigs")),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("DbConfig"));

builder.Services.AddServicesLib(builder.Configuration);
builder.Services.AddControllers();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI((options) =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/api", "api");
});
app.Run();