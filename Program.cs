using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EF.Models;

var builder = WebApplication.CreateBuilder(args);

//Esta linea solo sirve para la base de datos en memoria
//builder.Services.AddDbContext<TaskContext>(p => p.UseInMemoryDatabase("TaskDB"));
builder.Services.AddSqlServer<TaskContext>(builder.Configuration.GetConnectionString("SQLServer"));



var app = builder.Build();

//creamos un endpoit con estas lineas

app.MapGet("/", () => "Hello World!");



//con este mapeo, compruebo que puedo conectarme a una base de datos Real

app.MapGet("/dbconexion", async ([FromServices] TaskContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria:" + dbContext.Database.IsInMemory());
});

app.MapGet("/api/tareas", async ([FromServices] TaskContext dbcontext) =>
{
    //return Results.Ok(dbcontext.Tasks.Include(x => x.Categoty).Where(p => p.PriorityTask == Priority.medium));
    return Results.Ok(dbcontext.Tasks.Include(x => x.Categoty));

});

app.MapPost("/api/create", async ([FromServices] TaskContext dbcontext, [FromBody] EF.Models.Task tarea) =>
{
    tarea.Id = Guid.NewGuid();
    tarea.Date = DateTime.Now;
    //await dbcontext.AddAsync(tarea);
    await dbcontext.Tasks.AddAsync(tarea);

    await dbcontext.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/api/tareas/{id}", async ([FromServices] TaskContext dbcontext, [FromBody] EF.Models.Task tarea, [FromRoute] Guid id) =>
{
    var tareaActual = dbcontext.Tasks.Find(id);
    if (tareaActual != null)
    {
        tareaActual.Categoty = tarea.Categoty;
        tareaActual.Title = tarea.Title;
        tareaActual.PriorityTask = tarea.PriorityTask;
        tareaActual.Description = tarea.Description;

        await dbcontext.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TaskContext dbcontext, [FromRoute] Guid id) =>
{
    var tareaActual = dbcontext.Tasks.Find(id);
    if (tareaActual != null)
    {
        dbcontext.Remove(tareaActual);
        await dbcontext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});

app.Run();
