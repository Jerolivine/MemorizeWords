using MemorizeWords.Api;
using MemorizeWords.Context.EFCore;
using MemorizeWords.Infrastructure.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSwagger();

builder.Services.AddDbContext<MemorizeWordsDbContext>();

var app = builder.Build();

app.ConfigureSwagger();

ApiInitializer.Initialize(app);

app.Run();