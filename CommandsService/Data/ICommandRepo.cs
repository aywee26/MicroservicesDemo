﻿using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepo
{
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool DoesPlatformExist(int platformId);
    bool DoesExternalPlatformExist(int externalPlatformId);

    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command? GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}
