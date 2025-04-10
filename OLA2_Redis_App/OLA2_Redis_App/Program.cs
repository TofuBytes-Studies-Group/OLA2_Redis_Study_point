using Microsoft.OpenApi.Models;
using OLA2_Redis_App.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Redis API",
        Version = "v1",
        Description = "This API allows CRUD operations with Redis. It uses TTL for temporary data and demonstrates custom user permissions via ACLs.\n \n" +
                      "## remember: VALUE will be the actual name of the user whereas KEY is the parameter we use to identify the user in Redis.\n"
    });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("127.0.0.1:6379,password=yourStrongPassword"));
builder.Services.AddScoped<RedisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();