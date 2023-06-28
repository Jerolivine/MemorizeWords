using MemorizeWords.Api;
using MemorizeWords.Infrastructure.Configuration;
using MemorizeWords.Infrastructure.Persistance.Context.EFCore;
using MemorizeWords.Infrastructure.Transversal.Exception;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();

builder.Services.AddDbContext<MemorizeWordsDbContext>();

var app = builder.Build();

app.AddExceptionMiddieware();
app.InitializeApis();
app.ConfigureSwagger();
app.ConfigureCORS();

app.Run();