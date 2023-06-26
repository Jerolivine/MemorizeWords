using MemorizeWords.Api;
using MemorizeWords.Context.EFCore;
using MemorizeWords.Infrastructure.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();

builder.Services.AddDbContext<MemorizeWordsDbContext>();

var app = builder.Build();

app.InitializeApis();
app.ConfigureSwagger();
app.ConfigureCORS();

app.Run();