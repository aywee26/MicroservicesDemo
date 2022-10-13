using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Platform> Platforms => Set<Platform>();
	public DbSet<Command> Commands => Set<Command>();

	protected override void OnModelCreating(ModelBuilder modelBuidler)
	{
		modelBuidler
			.Entity<Platform>()
			.HasMany(p => p.Commands)
			.WithOne(p => p.Platform!)
			.HasForeignKey(p => p.PlatformId);

		modelBuidler
			.Entity<Command>()
			.HasOne(p => p.Platform)
			.WithMany(p => p.Commands)
			.HasForeignKey(p => p.PlatformId);
	}
}
