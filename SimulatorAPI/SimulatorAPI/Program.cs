using Microsoft.EntityFrameworkCore;
using SimulatorAPI.Context;
using SimulatorAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
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

app.UseHttpsRedirection();
app.UseCors(p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

// List Groups 
app.MapGet("/api/teams/groups", async (SimulatorContext context) =>
{
    var teams = await context.Teams.ToListAsync();
    var groups = teams.GroupBy(p => p.Group)
        .OrderBy(p => p.Key)
        .Select(p => p.Select(p => p));

    return Results.Ok(groups);
});

// FindAll
app.MapGet("/api/teams", async (SimulatorContext context) =>
{
    var teams = await context.Teams.ToListAsync();

    return Results.Ok(teams);
});

//FindById
app.MapGet("/api/teams/{id}", async (SimulatorContext context) =>
{
    var team = await context.Teams.FindAsync();

    return Results.Ok(team);
});

app.MapDelete("/api/teams/{id}", async (SimulatorContext context, Guid id) =>
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

app.MapPost("/api/teams", async (SimulatorContext context, Team team) =>
{
    await context.Teams.AddAsync(team);
    await context.SaveChangesAsync();

    return Results.Ok(team);
});

app.MapPut("/api/teams/{id}", async (SimulatorContext context, Team team) =>
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


app.Run();