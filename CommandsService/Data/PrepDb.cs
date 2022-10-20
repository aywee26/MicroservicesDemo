﻿using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

        var platforms = grpcClient.ReturnAllPlatforms();

        SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
    }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("Seeding new platforms...");

        foreach (var plat in platforms)
        {
            if (!repo.DoesExternalPlatformExist(plat.ExternalId))
            {
                repo.CreatePlatform(plat);
            }
            repo.SaveChanges();
        }
    }
}
