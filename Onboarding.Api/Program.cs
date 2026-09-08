var Tasks = new List<TaskItem>();
static int _nextId = 1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
app.MapGet("/add", (int x,int y ) => x + y);

app.MapGet("/tasks",() => Tasks);
app.MapGet("/tasks/{id}",(int id) =>
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);

        if (task is null)
            return Results.NotFound();

        return Results.Ok(task);
    });

app.MapPost("/tasks", (CreateTaskRequest request) =>
    {
        var task = new TaskItem(
           _nextId,
            request.Title,
            false
            );
        _nextId++;
        Tasks.Add(task);
        return Results.Created($"/tasks/{task.Id}", task);
    });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public record CreateTaskRequest(string Title);