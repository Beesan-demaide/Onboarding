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
        if (string.IsNullOrWhiteSpace(request.Title))
            return Results.BadRequest();

        var task = new TaskItem
        {
            Title = request.Title,
           IsDone = false
        };
        db.Tasks.Add(task);

        await db.SaveChangesAsync();

        return Results.Created($"/tasks/{task.Id}", task);
    });

app.MapPut("/tasks/{id}",async (int id, UpdatedTaskRequest request,TaskDbContext db) =>
    {
        var task =await db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if(task is null)
            return Results.NotFound();

    if (request.Title != null && string.IsNullOrWhiteSpace(request.Title))
            return Results.BadRequest();

        if(request.Title != null)
        task.Title = request.Title;

        if(request.IsDone.HasValue)
        task.IsDone = request.IsDone.Value;
        
        await db.SaveChangesAsync();

        return Results.Ok(task);
    });
app.MapDelete("/tasks/{id}",async (int id ,TaskDbContext db) =>
    {
        var task = await db.Tasks.FirstOrDefaultAsync(task => task.Id == id);

        if (task is null)
            return Results.NotFound();

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();

        return Results.NoContent();
    });
app.Run();

public record CreateTaskRequest(string Title);
public record UpdatedTaskRequest(string? Title,bool? IsDone);