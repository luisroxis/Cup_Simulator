using Microsoft.EntityFrameworkCore;
using SimulatorAPI.Context;
using SimulatorAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SimulatorContext>(
    options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("ServerConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// FindAll
app.MapGet("/teams", async (SimulatorContext context) =>
{
    var teams = await context.Teams.ToListAsync();

    return Results.Ok(teams);
});

//FindById
app.MapGet("/teams/{id}", async (SimulatorContext context) =>
{
    var team = await context.Teams.FindAsync();

    return Results.Ok(team);
});

app.MapDelete("/teams/{id}", async (SimulatorContext context, Guid id) =>
{
    var dbTeam = await context.Teams.FindAsync(id);

    if (dbTeam == null)
    {
        return Results.NotFound();
    }

    context.Teams.Remove(dbTeam);
    await context.SaveChangesAsync();

    return Results.NoContent();

});

app.MapPost("/teams", async (SimulatorContext context, Team team) =>
{
    await context.Teams.AddAsync(team);
    await context.SaveChangesAsync();

    return Results.Ok(team);
});

app.MapPut("/teams/{id}", async (SimulatorContext context, Team team) =>
{
    var dbTeam = await context.Teams.FindAsync(team.Id);

    if (dbTeam == null)
    {
        return Results.NotFound();
    }

    dbTeam.Name = team.Name;
    dbTeam.Img = team.Img;

    context.Teams.Update(dbTeam);
    await context.SaveChangesAsync();

    return Results.Ok(dbTeam);
});

app.UseHttpsRedirection();

app.Run();