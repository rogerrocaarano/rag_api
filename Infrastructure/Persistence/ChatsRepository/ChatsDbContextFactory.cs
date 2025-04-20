using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SQLitePCL;

namespace Infrastructure.Persistence.ChatsRepository;

public class ChatsDbContextFactory : IDesignTimeDbContextFactory<ChatsDb>
{
    static ChatsDbContextFactory()
    {
        Batteries.Init();
    }
    public ChatsDb CreateDbContext(string[] args)
    {
        // Construir la configuración para leer el appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("ChatsDbContextFactory.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Obtener la cadena de conexión desde la configuración
        var connectionString = configuration.GetConnectionString("ChatsDb") 
                               ?? throw new InvalidOperationException("Connection string 'ChatsDb' not found.");

        // Configurar las opciones del DbContext
        var optionsBuilder = new DbContextOptionsBuilder<ChatsDb>();
        optionsBuilder.UseSqlite(connectionString);

        return new ChatsDb(optionsBuilder.Options);
    }
}