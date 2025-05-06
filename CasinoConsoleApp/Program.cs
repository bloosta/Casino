using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Core.Handlers;
using CasinoConsoleApp.Core.Security;
using CasinoConsoleApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opts.SerializerOptions.WriteIndented = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();


builder.Services.AddScoped<ICommandHandler<AddUserCommand>, AddUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LoginUserCommand>, LoginUserCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateClientCommand>, CreateClientCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateClientCommand>, UpdateClientCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteClientCommand>, DeleteClientCommandHandler>();
builder.Services.AddScoped<ICommandHandler<SearchClientsCommand>, SearchClientsCommandHandler>();
builder.Services.AddScoped<ICommandHandler<AddGameCommand>, AddGameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateGameCommand>, UpdateGameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteGameCommand>, DeleteGameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<SearchGamesCommand>, SearchGamesCommandHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.MapPost("/api/users/register", async (AddUserCommand cmd, ICommandHandler<AddUserCommand> h) =>
{
    await h.HandleAsync(cmd);
    return Results.Ok();
});

app.MapPost("/api/users/login", async (
    LoginUserCommand cmd,
    ICommandHandler<LoginUserCommand> h,
    ICurrentUserContext ctx) => 
{
    await h.HandleAsync(cmd);
    return ctx.IsAuthenticated ? Results.Ok() : Results.Unauthorized();
});

app.MapPost("/api/clients", async (CreateClientCommand cmd, ICommandHandler<CreateClientCommand> h) =>
{
    await h.HandleAsync(cmd);
    return Results.Ok();
});

app.MapPut("/api/clients/{id:int}", async (int id, UpdateClientCommand cmd, ICommandHandler<UpdateClientCommand> h) =>
{

var updateCmd = new UpdateClientCommand
{
    Id = id,
    Name = cmd.Name,
    UseRawSql = cmd.UseRawSql
};
    await h.HandleAsync(updateCmd);
    return Results.Ok();
});

app.MapDelete("/api/clients/{id:int}", async (int id, ICommandHandler<DeleteClientCommand> h) =>
{
    await h.HandleAsync(new DeleteClientCommand { Id = id });
    return Results.Ok();
});

app.MapGet("/api/clients", async (
    [FromQuery] string? nameFilter,
    [FromQuery] bool useRawSql,
    ApplicationDbContext db) =>
{
    List<Client> result;

    if (useRawSql)
    {
        var sql = "SELECT * FROM Clients";
        if (!string.IsNullOrWhiteSpace(nameFilter))
            sql += " WHERE Name LIKE {0}";
        result = await db.Clients
            .FromSqlRaw(sql, $"%{nameFilter}%")
            .ToListAsync();
    }
    else
    {
        // EF‑Core
        var q = db.Clients.AsQueryable();
        if (!string.IsNullOrWhiteSpace(nameFilter))
            q = q.Where(c => EF.Functions.Like(c.Name, $"%{nameFilter}%"));
        result = await q.ToListAsync();
    }

    return Results.Ok(result);
});

app.MapPost("/api/games", async (AddGameCommand cmd, ICommandHandler<AddGameCommand> h) =>
{
    await h.HandleAsync(cmd);
    return Results.Ok();
});

app.MapPut("/api/games/{id:int}", async (
    int id,
    UpdateGameCommand cmd,
    ICommandHandler<UpdateGameCommand> h) =>
{
    var updateGameCmd = new UpdateGameCommand
    {
        Id = id,
        PlayedAt = cmd.PlayedAt,
        Type = cmd.Type,
        ClientIds = cmd.ClientIds,
        UseRawSql = cmd.UseRawSql
    };
    await h.HandleAsync(updateGameCmd);
    return Results.Ok();
});


app.MapDelete("/api/games/{id:int}", async (int id, ICommandHandler<DeleteGameCommand> h) =>
{
    await h.HandleAsync(new DeleteGameCommand { Id = id });
    return Results.Ok();
});

app.MapGet("/api/games", async (
        [FromServices] ApplicationDbContext db,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? typeFilter,
        [FromQuery] int? clientId) =>
{
    var query = db.Games.Include(g => g.Players).AsQueryable();

    if (from.HasValue)
        query = query.Where(g => g.PlayedAt >= from.Value);
    if (to.HasValue)
        query = query.Where(g => g.PlayedAt <= to.Value);
    if (!string.IsNullOrWhiteSpace(typeFilter))
        query = query.Where(g => EF.Functions.Like(g.Type, $"%{typeFilter}%"));
    if (clientId.HasValue)
        query = query.Where(g => g.Players.Any(p => p.Id == clientId.Value));

    var result = await query
            .Select(g => new
            {
                g.Id,
                g.PlayedAt,
                g.Type,
                PlayerIds = g.Players.Select(p => p.Id).ToList()
            })
            .ToListAsync();

    return Results.Ok(result);
});


app.Run();
