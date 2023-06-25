using MemorizeWords.Api;
using MemorizeWords.Context.EFCore;
using MemorizeWords.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();
builder.ConfigureCORS();

builder.Services.AddDbContext<MemorizeWordsDbContext>();

var app = builder.Build();

app.ConfigureSwagger();
app.ConfigureCORS();

ApiInitializer.Initialize(app);

app.Run();