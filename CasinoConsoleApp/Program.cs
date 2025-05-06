using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CasinoConsoleApp.Data;
using Microsoft.Extensions.Configuration;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
{
<<<<<<< Updated upstream
config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
})
    .ConfigureServices((context, services) =>
=======
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
    return Results.Ok(new { message = "Клиент добавлен" });
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
    return Results.Ok(new { message = "Клиент обновлен" });
});

app.MapDelete("/api/clients/{id:int}", async (int id, ICommandHandler<DeleteClientCommand> h) =>
{
    await h.HandleAsync(new DeleteClientCommand { Id = id });
    return Results.Ok(new { message = "Клиент удален" });
});

app.MapGet("/api/clients", async (
    [FromQuery] string? nameFilter,
    [FromQuery] bool useRawSql,
    ApplicationDbContext db) =>
{
    List<Client> result;

    if (useRawSql)
>>>>>>> Stashed changes
    {
        var conn = context.Configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(conn));

        // TODO: здесь зарегистрировать все ICommandHandler<> 
    })
    .Build();

<<<<<<< Updated upstream
using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.Migrate();

Console.WriteLine("CasinoConsoleApp запущено. Ожидание команд...");
host.Run();
=======
app.MapPost("/api/games", async (AddGameCommand cmd, ICommandHandler<AddGameCommand> h) =>
{
    await h.HandleAsync(cmd);
    return Results.Ok(new { message = "Игра добавлена" });
});

app.MapPut("/api/games/{id:int}", async (
        int id,
        UpdateGameCommand body,
        ICommandHandler<UpdateGameCommand> handler) =>
{
    // собираем окончательную команду, подставляя id из URL
    var updateGameCmd = new UpdateGameCommand
    {
        Id = id,
        PlayedAt = body.PlayedAt,
        Type = body.Type,
        ClientIds = body.ClientIds,
        UseRawSql = body.UseRawSql
    };

    // вызываем ваш handler, чтобы действительно выполнить UPDATE
    await handler.HandleAsync(updateGameCmd);

    return Results.Ok(new { message = "Игра обновлена" });
});


app.MapDelete("/api/games/{id:int}", async (int id, ICommandHandler<DeleteGameCommand> h) =>
{
    await h.HandleAsync(new DeleteGameCommand { Id = id });
    return Results.Ok(new { message = "Игра удалена" });
});

app.MapGet("/api/games", async (
        [FromServices] ApplicationDbContext db,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? typeFilter,
        [FromQuery] int? clientId) =>
{
    var query = db.Games
                  .Include(g => g.Players)
                  .AsQueryable();

    if (from.HasValue)
        query = query.Where(g => g.PlayedAt >= from.Value);
    if (to.HasValue)
        query = query.Where(g => g.PlayedAt <= to.Value);
    if (!string.IsNullOrWhiteSpace(typeFilter))
        query = query.Where(g => EF.Functions.Like(g.Type, $"%{typeFilter}%"));
    if (clientId.HasValue)
        query = query.Where(g => g.Players.Any(p => p.Id == clientId.Value));

    // Проекция в анонимный DTO
    var result = await query
        .Select(g => new
        {
            g.Id,
            g.PlayedAt,
            g.Type,
            Players = g.Players
                       .Select(p => new
                       {
                           p.Id,
                           p.Name
                       })
                       .ToList()
        })
        .ToListAsync();

    return Results.Ok(result);
});


app.Run();
>>>>>>> Stashed changes
