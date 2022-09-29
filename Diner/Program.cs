using ServicesLib;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServicesLib(builder.Configuration);
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();
app.Run();