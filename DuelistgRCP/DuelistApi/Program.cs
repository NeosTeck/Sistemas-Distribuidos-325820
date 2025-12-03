using MongoDB.Driver;
using DuelistApi.Repositories;
using DuelistApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var connectionString = builder.Configuration["MongoDB:ConnectionString"];
    var dbName = builder.Configuration["MongoDB:DatabaseName"];
    var client = new MongoClient(connectionString);
    return client.GetDatabase(dbName);
});

builder.Services.AddScoped<IDuelistRepository, DuelistRepository>();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<DuelistService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
