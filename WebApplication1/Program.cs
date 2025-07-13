using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/say-go", () => "Go, World!+++").WithName("SayGo");
app.MapGet("/say-hello", () => "Hello, World!").WithName("SayHello");
app.MapGet("/say-goodbye", () => "Goodbye, World!").WithName("SayGoodbye");

Console.WriteLine("It is running on: " + Environment.MachineName);

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 50).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                )
                {
                    MachineName = Environment.MachineName
                })
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string MachineName { get; set; } = string.Empty;
}