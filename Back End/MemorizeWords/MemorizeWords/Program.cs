using MemorizeWords.Infrastructure.Configuration;
using MemorizeWords.Infrastructure.Configuration.DbConfiguration.EfCore;
using MemorizeWords.Infrastructure.Configuration.ServiceInjector;
using MemorizeWords.Infrastructure.Transversal.Exception;
using MemorizeWords.Quartz;
using MemorizeWords.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();
builder.ConfigureEfCore();
builder.InjectServices();
builder.ConfigureJobs();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();

app.MapControllers();

app.AddExceptionMiddieware();
app.ConfigureSwagger();
app.ConfigureCORS();

app.MapHub<UserGuessedWordsHub>("/userGuessedWordsHub");

app.Run();