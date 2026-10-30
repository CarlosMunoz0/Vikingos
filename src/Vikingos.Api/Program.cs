using System.Text.Json.Serialization;
using Vikingos.Api.Repositories;
using Vikingos.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IVikingoRepository, InMemoryVikingoRepository>();
builder.Services.AddSingleton<IValhallaClassifier, ValhallaClassifier>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
