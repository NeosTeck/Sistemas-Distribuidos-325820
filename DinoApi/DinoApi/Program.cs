using DinoApi.Services;
using DinoApi.Repositories;
using DinoApi.Infrastructure;
using SoapCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSoapCore();
builder.Services.AddScoped<IDinoService, DinoService>();
builder.Services.AddScoped<IDinoRepo, DinoRepo>();

builder.Services.AddDbContext<DinoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

var app = builder.Build();

app.UseSoapEndpoint<IDinoService>("/DinoService.svc", new SoapEncoderOptions());
app.Run();

