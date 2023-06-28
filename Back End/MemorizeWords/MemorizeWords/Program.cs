using MemorizeWords.Api;
using MemorizeWords.Context.EFCore;
using MemorizeWords.Infrastructure.Configuration;
using MemorizeWords.Infrastructure.Transversal.Exception;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();

builder.Services.AddDbContext<MemorizeWordsDbContext>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.InitializeApis();
app.ConfigureSwagger();
app.ConfigureCORS();

app.Run();