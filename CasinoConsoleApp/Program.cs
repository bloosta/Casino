using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Handlers;
using CasinoConsoleApp.Core.Security;
using CasinoConsoleApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


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

app.MapPost("/api/users/login", async (LoginUserCommand cmd, ICommandHandler<LoginUserCommand> h) =>
{
    await h.HandleAsync(cmd);
    var ctx = app.Services.GetRequiredService<ICurrentUserContext>();
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

app.MapGet("/api/clients", async ([FromQuery] string? nameFilter, [FromQuery] bool useRawSql,
    ICommandHandler<SearchClientsCommand> h) =>
{
    var cmd = new SearchClientsCommand { NameFilter = nameFilter, UseRawSql = useRawSql };
    await h.HandleAsync(cmd);
    return Results.Ok();
});

app.MapPost("/api/games", async (AddGameCommand cmd, ICommandHandler<AddGameCommand> h) =>
{
    await h.HandleAsync(cmd);
    return Results.Ok();
});

app.MapPut("/api/games/{id:int}", async (int id, UpdateGameCommand cmd, ICommandHandler<UpdateGameCommand> h) =>
{
    var updateGameCmd = new UpdateGameCommand
    {
        Id = id,
        PlayedAt = cmd.PlayedAt,
        Type = cmd.Type,
        ClientIds = cmd.ClientIds,
        UseRawSql = cmd.UseRawSql
    };
    return Results.Ok();
});

app.MapDelete("/api/games/{id:int}", async (int id, ICommandHandler<DeleteGameCommand> h) =>
{
    await h.HandleAsync(new DeleteGameCommand { Id = id });
    return Results.Ok();
});

app.MapGet("/api/games", async ([FromQuery] DateTime? from, [FromQuery] DateTime? to,
    [FromQuery] string? typeFilter, [FromQuery] int? clientId, [FromQuery] bool useRawSql,
    ICommandHandler<SearchGamesCommand> h) =>
{
    var cmd = new SearchGamesCommand
    {
        From = from,
        To = to,
        TypeFilter = typeFilter,
        ClientId = clientId,
        UseRawSql = useRawSql
    };
    await h.HandleAsync(cmd);
    return Results.Ok();
});

app.Run();
