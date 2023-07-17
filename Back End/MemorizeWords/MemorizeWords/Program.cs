using MemorizeWords.Infrastructure.Configuration;
using MemorizeWords.Infrastructure.Configuration.DbConfiguration.EfCore;
using MemorizeWords.Infrastructure.Configuration.ServiceInjector;
using MemorizeWords.Infrastructure.Transversal.Exception;
using MemorizeWords.Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();
builder.ConfigureEfCore();
builder.InjectServices();
builder.ConfigureJobs();
builder.ConfigureLog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();
app.ConfigureLog(builder.Configuration);

app.AddExceptionMiddieware();
app.ConfigureSwagger();
app.ConfigureCORS();

app.Run();