using BoarDo.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BoarDo.Server.Database;

public class BoarDoContext : DbContext
{
	public DbSet<OAuthClient> OAuthClients { get; set; }
	
	public string DbPath { get; }


	public BoarDoContext()
	{
		var path = AppDomain.CurrentDomain.BaseDirectory;
		DbPath = Path.Join(path, "boardo.db");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options) =>
		options.UseSqlite($"Data Source={DbPath}");
}