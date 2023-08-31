using BoarDo.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BoarDo.Server.Database;

public class BoarDoContext : DbContext
{
    public BoarDoContext()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        DbPath = Path.Join(path, "boardo.db");
    }

    public DbSet<OAuthClient> OAuthClients { get; set; }

    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<OAuthClient>().Property(c => c.Id).HasConversion<EnumToStringConverter<OAuthClientProvider>>();
    }
}