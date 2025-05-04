using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CasinoConsoleApp.Data;
using Microsoft.Extensions.Configuration;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
{
config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
})
    .ConfigureServices((context, services) =>
    {
        var conn = context.Configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(conn));

        // TODO: здесь зарегистрировать все ICommandHandler<>
    })
    .Build();

using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
db.Database.Migrate();

Console.WriteLine("CasinoConsoleApp запущено. Ожидание команд...");
host.Run();
