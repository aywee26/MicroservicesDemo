using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, isProduction);
    }

    private static void SeedData(AppDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("Applying migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't apply migrations: {ex.Message}");
            }

        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("Seeding data...");
            context.Platforms.AddRange(
                new Platform() { Name = ".NET", Publisher = ".NET Foundation", Cost = "Free" },
                new Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Data seeding is not needed.");
        }
    }
}
