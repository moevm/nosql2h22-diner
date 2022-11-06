using ServicesLib;
using UtilsLib.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("DbConfig"));

builder.Services.AddServicesLib(builder.Configuration);
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();
app.Run();