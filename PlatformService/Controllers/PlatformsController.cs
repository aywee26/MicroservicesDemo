using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
	private readonly IPlatformRepo _repository;
	private readonly IMapper _mapper;
	private readonly ICommandDataClient _commandDataClient;

	public PlatformsController(
		IPlatformRepo repository,
		IMapper mapper,
		ICommandDataClient commandDataClient)
	{
		_repository = repository;
		_mapper = mapper;
		_commandDataClient = commandDataClient;
	}

	[HttpGet]
	public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
	{
		Console.WriteLine("Getting Platforms...");
		var platformsAsModels = _repository.GetAllPlatforms();
		var platformsAsDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platformsAsModels);
		return Ok(platformsAsDtos);
	}

	[HttpGet("{id}", Name = "GetPlatformById")]
	public ActionResult<PlatformReadDto> GetPlatformById(int id)
	{
		var platformModel = _repository.GetPlatformById(id);
		if (platformModel is null)
		{
			return NotFound();
		}
		var platformDto = _mapper.Map<PlatformReadDto>(platformModel);
		return Ok(platformDto);
	}

	[HttpPost]
	public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformDto)
	{
		var platformModel = _mapper.Map<Platform>(platformDto);
		_repository.CreatePlatform(platformModel);
		_repository.SaveChanges();

		var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

		try
		{
			await _commandDataClient.SendPlatformToCommand(platformReadDto);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Could not send platform synchronously: {ex.Message}");
		}

		return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
	}
}
