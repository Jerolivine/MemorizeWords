using MemorizeWords.Infrastructure.Configuration;
using MemorizeWords.Infrastructure.Configuration.DbConfiguration.EfCore;
using MemorizeWords.Infrastructure.Configuration.ServiceInjector;
using MemorizeWords.Infrastructure.Transversal.Exception;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();
builder.ConfigureEfCore();
builder.InjectServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();

app.AddExceptionMiddieware();
//app.InitializeApis();
app.ConfigureSwagger();
app.ConfigureCORS();

app.Run();