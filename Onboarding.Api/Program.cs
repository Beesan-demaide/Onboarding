using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

    
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

app.MapGet("/add", (int x,int y ) => x + y);

app.MapGet("/tasks", async (TaskDbContext db) =>
    {
        return await db.Tasks.ToListAsync();
    });
app.MapGet("/tasks/{id}",async(int id,TaskDbContext db) =>
    {
        var task = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task is null)
            return Results.NotFound();

        return Results.Ok(task);
    });

app.MapPost("/tasks", async (CreateTaskRequest request ,TaskDbContext db) =>
    {
        var task = new TaskItem
        {
            Title = request.Title,
           IsDone = false
        };
        db.Tasks.Add(task);

        await db.SaveChangesAsync();

        return Results.Created($"/tasks/{task.Id}", task);
    });


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public record CreateTaskRequest(string Title);