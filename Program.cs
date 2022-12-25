using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platzi.Models;

var builder = WebApplication.CreateBuilder(args);

//Esta linea solo sirve para la base de datos en memoria
//builder.Services.AddDbContext<TaskContext>(p => p.UseInMemoryDatabase("TaskDB"));
builder.Services.AddSqlServer<TaskContext>(builder.Configuration.GetConnectionString("SQLServer"));



var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//con este mapeo, compruebo que puedo conectarme a una base de datos Real

object value = app.MapGet("/dbconexion", async ([FromServices] TaskContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria:" + dbContext.Database.IsInMemory());
});

app.Run();
